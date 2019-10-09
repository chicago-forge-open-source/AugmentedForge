import os
import sys

dirname = os.path.dirname(__file__)

print("dirname " + dirname)

abspath = os.path.abspath(os.path.join(dirname, '..'))

print("abspath " + abspath)

sys.path.insert(0, abspath)

import light_control
