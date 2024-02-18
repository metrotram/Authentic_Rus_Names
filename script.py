import os
import re
import shutil


# Assets.HIGHWAY_NAME:0$Amity Highway&
# $ is \r, Carriage Return, ASCII Dec 13, 0x0D = &
def patch_section(section, filename):
    # Section: Assets.ALLEY_NAME, Assets.STREET_NAME, Assets.HIGHWAY_NAME, Assets.BRIDGE_NAME, Assets.DAM_NAME
    # Find all entries of the section

    # For Each entry:
    valid_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.: "
    # Advance until character is found not in regex
    #

    data = ""
    with open(filename, 'rb') as file:
        data = file.read()
    update_content = bytearray(data)

    invalid_chars_dollar = []
    invalid_chars_ampersand = []

    for i in range(0, 109):
        look_for = section + ":" + str(i)
        index = update_content.find(look_for.encode('utf-8'))
        # check if char at current index is in valid_chars
        while chr(update_content[index]) in valid_chars:
            print("Found valid char: " + chr(update_content[index]) + " at index: " + str(index))
            index += 1
        # ch = chr(update_content[index])
        invalid_chars_dollar.append(update_content[index])
        start_index = index + 1  # first index of actual name
        end_index = start_index
        while chr(update_content[end_index]) in valid_chars:
            end_index += 1
        invalid_chars_ampersand.append(update_content[index])

        name_length = end_index - start_index
        update_content[start_index:end_index] = b" " * name_length

    with open(filename, 'wb') as file:
        file.write(update_content)


"""def update_locale(filename):
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
"""


def patch_all_sections(locale):
    shutil.copy("original/" + locale + ".loc", "updated/" + locale + ".loc")
    patch_section("Assets.STREET_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.HIGHWAY_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.ALLEY_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.BRIDGE_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.DAM_NAME", "updated/" + locale + ".loc")


def patch_all_locales():
    patch_all_sections("en-US")
    patch_all_sections("de-DE")
    patch_all_sections("es-ES")
    patch_all_sections("fr-FR")
    patch_all_sections("it-IT")
    patch_all_sections("ja-JP")
    patch_all_sections("ko-KR")
    patch_all_sections("pl-PL")
    patch_all_sections("pt-BR")
    patch_all_sections("ru-RU")
    patch_all_sections("zh-HANS")
    patch_all_sections("zh-HANT")


if __name__ == '__main__':
    patch_all_locales()
    print("OK")
