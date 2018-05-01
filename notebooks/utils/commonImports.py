# Basics
import numpy as np
import pandas as pd
from copy import deepcopy

# Ploting
pd.options.display.float_format = '{:,.4f}'.format
import matplotlib.pyplot as plt
import seaborn as sns
from matplotlib.font_manager import FontProperties

sns.set_style('darkgrid')
titleFont = FontProperties(weight='bold', size=20)
axisFont = FontProperties(weight='bold', size=14)