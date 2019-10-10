using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.IotData;
using Amazon.IotData.Model;
using UnityEngine;

[Serializable]
public class ShadowThing
{
    public ShadowState state;
}

[Serializable]
public class ShadowState
{
    public ThingState desired;
    public ThingState reported;
}

[Serializable]
public class ThingState
{
    public string state;
    public string text;
}

public class Thing
{
    private readonly string _thingName;
    private readonly AmazonIotDataClient _dataClient;

    public Thing(string thingName)
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

        _thingName = thingName;
        _dataClient = new AmazonIotDataClient(awsAccessKeyId, awsSecretAccessKey, amazonIotDataConfig);
    }

    public async Task<ThingState> GetThing()
    {
        var getThingShadowRequest = new GetThingShadowRequest
        {
            ThingName = _thingName
        };

        var theThing = await _dataClient.GetThingShadowAsync(getThingShadowRequest, CancellationToken.None);
        var json = Encoding.UTF8.GetString(theThing.Payload.ToArray());
        var shadowThing = JsonUtility.FromJson<ShadowThing>(json);
        return shadowThing.state.reported;
    }

    public async Task UpdateThing(string desiredState)
    {
        var publishRequest = new PublishRequest
        {
            Topic = $"$aws/things/{_thingName}/shadow/update",
            Payload = new MemoryStream(
                Encoding.UTF8.GetBytes(
                    $"{{ \"state\" : {{ \"desired\" : {desiredState} }} }}"
                )
            ),
            Qos = 1
        };

        await _dataClient.PublishAsync(publishRequest);
    }
}