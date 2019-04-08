import sys
import os

folders = ["/Users/david.vesely/git-projects", "D:\\GitProjects"]
for folder in folders:
    for e in ["krypl-project", os.path.join("krypl-project", "notebooks")]:
        path = os.path.join(folder, e)
        sys.path.append(path)
