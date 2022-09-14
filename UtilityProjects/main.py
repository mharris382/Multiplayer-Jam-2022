# This is a sample Python script.
import os
from pathlib import Path


# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.



def print_hi(name):
    # Use a breakpoint in the code line below to debug your script.
    print(f'Hi, {name}')  # Press Ctrl+F8 to toggle the breakpoint.


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    path = Path(os.getcwd())
    print(path.parent)
    target = f"{path.parent}\\Steam Eagles GDD\\Game Design Document"
    print(os.path.exists(target))
    for root, dirs, files in os.walk(target, topdown=False):
        for file in files:
            print(f"{root}\t\t\t{file}")

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
