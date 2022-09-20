import os
from pathlib import Path


def remove_tags_all(tag: str):
    path = Path(os.getcwd())
    print(path.parent)
    target = f"{path.parent}\\Steam Eagles GDD\\Game Design Document"
    print(os.path.exists(target))
    for root, dirs, files in os.walk(target, topdown=False):
        for file in files:
            path = os.path.join(root, file)
            remove_tag(path, "#readme")

def remove_tag(file_path: str, tag: str):
    lines = []
    with open(file_path, "r+") as f:
        for line in f:
            if tag in line:
                lines.append(line.replace(tag, ""))
            else:
                lines.append(line)
    fl = open(file_path, "w")
    for line in lines:
        fl.write(line)
    fl.close()