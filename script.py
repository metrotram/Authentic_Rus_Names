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
        index = update_content.find(look_for.encode('utf-8')) + len(look_for)
        # check if char at current index is in valid_chars
        while chr(update_content[index]) in valid_chars:
            # print("Found valid char: " + chr(update_content[index]) + " at index: " + str(index))
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
        #print(update_content[start_index:end_index])

    with open(filename, 'wb') as file:
        file.write(update_content)


def patch_all_sections(locale):
    shutil.copy("original/" + locale + ".loc", "updated/" + locale + ".loc")
    patch_section("Assets.STREET_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.HIGHWAY_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.ALLEY_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.BRIDGE_NAME", "updated/" + locale + ".loc")
    patch_section("Assets.DAM_NAME", "updated/" + locale + ".loc")


def patch_all_locales():
    patch_all_sections("en-US")
    # Other languages are not yet working
    """patch_all_sections("de-DE")
    patch_all_sections("es-ES")
    patch_all_sections("fr-FR")
    patch_all_sections("it-IT")
    patch_all_sections("ja-JP")
    patch_all_sections("ko-KR")
    patch_all_sections("pl-PL")
    patch_all_sections("pt-BR")
    patch_all_sections("ru-RU")
    patch_all_sections("zh-HANS")
    patch_all_sections("zh-HANT")"""


if __name__ == '__main__':
    patch_all_locales()
    print("OK")
