# Basics
import numpy as np
import pandas as pd
from copy import deepcopy
from utils.common import *
from utils.date import *
from utils.plot import *

# Ploting
pd.options.display.float_format = '{:,.4f}'.format
import matplotlib.pyplot as plt
import seaborn as sns
from matplotlib.font_manager import FontProperties

sns.set_style('darkgrid')
title_font = FontProperties(weight='bold', size=20)
axis_font = FontProperties(weight='bold', size=14)