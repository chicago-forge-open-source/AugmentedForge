import os
import sys

dirname = os.path.dirname(__file__)

abspath = os.path.abspath(os.path.join(dirname, '..'))

sys.path.insert(0, abspath)

import light_control
