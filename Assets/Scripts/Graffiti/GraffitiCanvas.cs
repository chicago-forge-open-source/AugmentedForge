using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.IotData;
using Amazon.IotData.Model;
using UnityEngine;

namespace Graffiti
{
    [Serializable]
    public class ShadowThing
    {
        public ShadowState state;
    }

    [Serializable]
    public class ShadowState
    {
        public GraffitiCanvasState desired;
        public GraffitiCanvasState reported;
    }

    [Serializable]
    public class GraffitiCanvasState
    {
        public string color;
    }

    public class GraffitiCanvas
    {
        private readonly AmazonIotDataClient _dataClient;

        public GraffitiCanvas()
        {
            var fileText = Resources.Load<TextAsset>("accesskeys").text;
            var lines = fileText.Split('\n');
            var secrets = lines[1].Split(',');
            var awsAccessKeyId = secrets[0].Trim();
            var awsSecretAccessKey = secrets[1].Trim();
            var amazonIotDataConfig = new AmazonIotDataConfig
            {
                ServiceURL = "https://a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com/"
            };
            _dataClient = new AmazonIotDataClient(awsAccessKeyId, awsSecretAccessKey, amazonIotDataConfig);
        }

        public async Task<GraffitiCanvasState> GetIoTThing()
        {
            var getThingShadowRequest = new GetThingShadowRequest
            {
                ThingName = "Flounder"
            };

            var theThing = await _dataClient.GetThingShadowAsync(getThingShadowRequest, CancellationToken.None);
            var json = Encoding.UTF8.GetString(theThing.Payload.ToArray());
            var shadowThing = JsonUtility.FromJson<ShadowThing>(json);
            return shadowThing.state.reported;
        }

        public async Task UpdateMagicWallColor(Color color)
        {
            var publishRequest = new PublishRequest
            {
                Topic = "$aws/things/Flounder/shadow/update",
                Payload = new MemoryStream(
                    Encoding.UTF8.GetBytes(
                        $"{{ \"state\" : {{ \"desired\" : {{ \"color\":\"#{ColorUtility.ToHtmlStringRGBA(color)}\"}} }} }}"
                    )
                ),
                Qos = 1
            };

            await _dataClient.PublishAsync(publishRequest);
        }
    }
}