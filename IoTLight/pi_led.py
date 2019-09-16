#!/usr/bin/python

import os
import sys


def control_led(state):
    if state == 'on':
        os.system('echo none | sudo tee /sys/class/leds/led0/trigger')
        os.system('echo 1 | sudo tee /sys/class/leds/led0/brightness')

    elif state == 'off':
        os.system('echo none | sudo tee /sys/class/leds/led0/trigger')
        os.system('echo 0 | sudo tee /sys/class/leds/led0/brightness')


if __name__ == '__main__':
    control_led(*sys.argv[1:])


