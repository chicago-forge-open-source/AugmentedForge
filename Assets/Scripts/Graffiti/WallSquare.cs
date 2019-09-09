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
        public WallSquareState desired;
        public WallSquareState reported;
    }

    [Serializable]
    public class WallSquareState
    {
        public string color;
    }

    public class WallSquare
    {
        public static async Task<WallSquareState> GetIoTThing()
        {
            var lines = File.ReadAllLines("accesskeys.csv");
            var secrets = lines[1].Split(',');
            var awsAccessKeyId = secrets[0];
            var awsSecretAccessKey = secrets[1];
            var amazonIotDataConfig = new AmazonIotDataConfig
            {
                ServiceURL = "https://a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com/"
            };

            using (var dataClient = new AmazonIotDataClient(awsAccessKeyId, awsSecretAccessKey, amazonIotDataConfig))
            {
                var getThingShadowRequest = new GetThingShadowRequest
                {
                    ThingName = "Flounder"
                };

                var theThing = await dataClient.GetThingShadowAsync(getThingShadowRequest, CancellationToken.None);

                var shadowThing =
                    JsonUtility.FromJson<ShadowThing>(Encoding.UTF8.GetString(theThing.Payload.ToArray()));
                return shadowThing.state.reported;
            }
        }

        public static async Task UpdateMagicWallColor(Color color)
        {
            var lines = File.ReadAllLines("accesskeys.csv");
            var secrets = lines[1].Split(',');
            var awsAccessKeyId = secrets[0];
            var awsSecretAccessKey = secrets[1];
            var amazonIotDataConfig = new AmazonIotDataConfig
            {
                ServiceURL = "https://a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com/"
            };
            using (var dataClient = new AmazonIotDataClient(awsAccessKeyId, awsSecretAccessKey, amazonIotDataConfig))
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
                
                await dataClient.PublishAsync(publishRequest);
            }
        }
    }
}