#!/usr/bin/python
import json

from AWSIoTPythonSDK.MQTTLib import AWSIoTMQTTShadowClient


class IoTLightPi(object):

    def json_encode(self, string):
        return json.dumps(string)

    def on_message(self, message, response, token):
        print(message)

    def getIoTThing(self):
        mqttc = AWSIoTMQTTShadowClient('1235')

        mqttc.configureEndpoint('a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com', 8883)
        mqttc.configureCredentials('./certificates/AmazonRootCA1.pem',
                                   './certificates/IoTLight.private.key',
                                   './certificates/IoTLight.cert.pem')
        mqttc.configureConnectDisconnectTimeout(10)
        mqttc.configureMQTTOperationTimeout(5)

        device_shadow = mqttc.createShadowHandlerWithName('IoTLight', True)

        shadow_message = {"state": {"reported": {"temperature": 65}, "desired": {"temperature": 75}}}
        shadow_message = json.dumps(shadow_message)

        device_shadow.on_message = self.on_message
        device_shadow.json_encode = self.json_encode

        print("About to connect")
        mqttc.connect()
        print("Connection happneed")

        # sending first shadow update
        print('Connected')
        device_shadow.shadowUpdate(shadow_message, self.on_message, 5)
        print('Shadow Update Sent')
        mqttc.disconnect()


if __name__ == '__main__':
    thing = IoTLightPi()
    thing.getIoTThing()
