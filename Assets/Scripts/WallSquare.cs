using System.IO;
using System.Threading.Tasks;
using Amazon.IotData;
using Amazon.IotData.Model;
using UnityEngine;

namespace DefaultNamespace
{
    public class WallSquare
    {
        public static async Task DoIoTThing()
        {
            var lines = File.ReadAllLines("accesskeys.csv");
            var secrets = lines[1].Split(',');

            Debug.Log("Started do thing");

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
                        System.Text.Encoding.UTF8.GetBytes(
                            "{ \"state\" : { \"desired\" : { \"flavor\":\"purple\"} } }"
                        )
                    ),
                    Qos = 1
                };

                Debug.Log("Before start new ");
                Debug.Log("Before publish");
                await dataClient.PublishAsync(publishRequest);

                Debug.Log("After publish");
            }
        }
    }
}