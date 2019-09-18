#!/usr/bin/python
import json
import time
import os
import subprocess
import sys

from AWSIoTPythonSDK.MQTTLib import AWSIoTMQTTShadowClient


class IoTCommunicator(object):

    def __init__(self, device):
        self.mqttc = AWSIoTMQTTShadowClient('IoTLight')
        self.mqttc.configureEndpoint('a2soq6ydozn6i0-ats.iot.us-west-2.amazonaws.com', 8883)
        self.mqttc.configureCredentials('./certificates/AmazonRootCA1.pem',
                                        './certificates/IoTLight.private.key',
                                        './certificates/IoTLight.cert.pem')
        self.mqttc.configureConnectDisconnectTimeout(10)
        self.mqttc.configureMQTTOperationTimeout(5)
        self.device_shadow = self.mqttc.createShadowHandlerWithName('IoTLight', True)
        self.device_shadow.on_message = self.on_message
        self.device_shadow.json_encode = self.json_encode
        self.device = device

    def json_encode(self, string):
        return json.dumps(string)

    def on_message(self, message, response, token):
        print(message)

    def on_delta(self, message, response, token):
        print("delta %s" % message)
        loaded_message = json.loads(message)
        new_state = loaded_message["state"]["state"]
        self.device.set_light(new_state)
        self.send_shadow_update()

    def start_communication(self):
        print("About to connect")
        self.mqttc.connect()
        print('Connected')
        self.device_shadow.shadowRegisterDeltaCallback(self.on_delta)

        loop_count = 0
        while True:
            self.send_shadow_update()
            loop_count += 1
            time.sleep(5)

    def send_shadow_update(self):
        message = {"state": {"reported": {"state": self.device.light_state}}}
        message_json = json.dumps(message)
        self.device_shadow.shadowUpdate(message_json, self.on_message, 5)
        print('Shadow Update Sent')
        print('Published state %s\n' % message_json)


class FakeIoTLightDevice(object):
    def __init__(self):
        self.light_state = "off"

    def set_light(self, state):
        self.light_state = state

    pass


class RealIoTLightDevice(object):
    def __init__(self):
        self.set_light("off")
        self._light_state = "off"

    def set_light(self, state):
        control_led(state)
        self._light_state = state

    @property
    def light_state(self):
        return self._light_state

    pass


def control_led(state):
    if state == 'on':
        os.system('echo none | sudo tee /sys/class/leds/led0/trigger')
        os.system('echo 1 | sudo tee /sys/class/leds/led0/brightness')

    elif state == 'off':
        os.system('echo none | sudo tee /sys/class/leds/led0/trigger')
        os.system('echo 0 | sudo tee /sys/class/leds/led0/brightness')


if __name__ == '__main__':
    arg = sys.argv[1:]

    if arg == ['test']:
        print("test mode")
        device = FakeIoTLightDevice()
    else:
        device = RealIoTLightDevice()

    thing = IoTCommunicator(device)
    thing.start_communication()