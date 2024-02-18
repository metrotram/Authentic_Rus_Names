import os
import re


def update_locale(filename):
    text = ""
    with open("original/" + filename, 'rb') as file:
        text = file.read()  # Read binary file content

    updated_content = bytearray(text)

    for i in range(0, 109):
        add_byte = 0
        if i > 9:
            add_byte = 1
        if i > 99:
            add_byte = 2
        lookfor = "Assets.HIGHWAY_NAME:" + str(i)
        index = updated_content.find(lookfor.encode('utf-8'))
        start_index = index + 22 + add_byte
        print(updated_content[start_index:index + 30])

        end_index = start_index
        for j in range(0, 30):
            if updated_content[start_index + j] == 32:
                print("Space found")
                end_index = start_index + j + len(" Highway")
                break
        print(updated_content[start_index:end_index])

        updated_content[start_index:end_index] = b" " * (end_index - start_index)

    with open("updated/" + filename, 'wb') as file:
        file.write(updated_content)


def update_dir():
    # Loop through all files in /original folder
    for filename in os.listdir("original"):
        if filename.endswith('.loc'):
            update_locale(filename)


if __name__ == '__main__':
    #update_dir()
    update_locale("en-US.loc")
