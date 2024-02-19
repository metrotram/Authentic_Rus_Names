import shutil

def patch_section(section, filename):
    lines = []
    updated_lines = []
    with open(filename, 'r', encoding="utf-8") as file:
        lines = file.readlines()

    for line in lines:
        if line.startswith(section):
            # Remove text between { and }
            start = line.find("{")
            end = line.find("}")
            if start != -1 and end != -1:
                line = line[:start] + "{" + (" " * (end-start)) + "}" + line[end+1:]
        updated_lines.append(line)

    with open(filename, 'w', encoding="utf-8") as file:
        file.writelines(updated_lines)


def patch_all_sections(locale):
    patch_section("Assets.STREET_NAME", "updated/" + locale + ".txt")
    patch_section("Assets.HIGHWAY_NAME", "updated/" + locale + ".txt")
    patch_section("Assets.ALLEY_NAME", "updated/" + locale + ".txt")
    patch_section("Assets.BRIDGE_NAME", "updated/" + locale + ".txt")
    patch_section("Assets.DAM_NAME", "updated/" + locale + ".txt")


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
