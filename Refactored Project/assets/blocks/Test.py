import os
from PIL import Image

def should_pixel_be_transparent(data):
    r = data[0]
    b = data[1]
    g = data[2]
    return r <= 10 and b <= 10 and g <= 10

def process_image(path):
    print(path)
    im = Image.open(path, mode='r')
    img = im.convert("RGBA")
    clear =(255,255,255,0)
    width = img.size[0]
    height = img.size[1]
    #pixel_values = list(img.getdata())
    
    for x in range(0, width):
        for y in range(0, height):
            data = img.getpixel((x,y))
            if should_pixel_be_transparent(data):
                img.putpixel((x,y), clear)
    #img.show()
    img.save(path)
            

if __name__ == '__main__':
    
    path = os.getcwd()
    print(path)
    for root, dirs, files in os.walk(".", topdown=False):
        for name in files:
            parts = os.path.splitext(name)
            ext =  parts[1]
            if ext != ".png":
                continue
            if "template" in name:
                print(f"found {name}")
                process_image(os.path.join(root, name))
            #print(os.path.splitext(os.path.join(root,name))
