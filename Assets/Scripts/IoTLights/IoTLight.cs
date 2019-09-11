using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.IotData;
using Amazon.IotData.Model;
using UnityEngine;

namespace IoTLights
{
    [Serializable]
    public class ShadowThing
    {
        public ShadowState state;
    }

    [Serializable]
    public class ShadowState
    {
        public IoTLightState desired;
        public IoTLightState reported;
    }

    [Serializable]
    public class IoTLightState
    {
        public string state;
    }

    public class IoTLight
    {
        private readonly AmazonIotDataClient _dataClient;
        public IoTLight()
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
        
        public async Task<IoTLightState> GetIoTThing()
        {
            var getThingShadowRequest = new GetThingShadowRequest
            {
                ThingName = "IoTLight"
            };

            var theThing = await _dataClient.GetThingShadowAsync(getThingShadowRequest, CancellationToken.None);
            var json = Encoding.UTF8.GetString(theThing.Payload.ToArray());
            var shadowThing = JsonUtility.FromJson<ShadowThing>(json);
            return shadowThing.state.reported;
        }
        
        public async Task UpdateLightState(string state)
        {
            var publishRequest = new PublishRequest
            {
                Topic = "$aws/things/IoTLight/shadow/update",
                Payload = new MemoryStream(
                    Encoding.UTF8.GetBytes(
                        $"{{ \"state\" : {{ \"desired\" : {{ \"state\":\"{state}\"}} }} }}"
                    )
                ),
                Qos = 1
            };

            await _dataClient.PublishAsync(publishRequest);
        }
    }
}