import json
import os
import re
from pathlib import Path


BASE_ROMAJI = {
    "あ": "a", "い": "i", "う": "u", "え": "e", "お": "o",
    "か": "ka", "き": "ki", "く": "ku", "け": "ke", "こ": "ko",
    "さ": "sa", "し": "shi", "す": "su", "せ": "se", "そ": "so",
    "た": "ta", "ち": "chi", "つ": "tsu", "て": "te", "と": "to",
    "な": "na", "に": "ni", "ぬ": "nu", "ね": "ne", "の": "no",
    "は": "ha", "ひ": "hi", "ふ": "fu", "へ": "he", "ほ": "ho",
    "ま": "ma", "み": "mi", "む": "mu", "め": "me", "も": "mo",
    "や": "ya", "ゆ": "yu", "よ": "yo",
    "ら": "ra", "り": "ri", "る": "ru", "れ": "re", "ろ": "ro",
    "わ": "wa", "を": "wo", "ん": "n",
    "が": "ga", "ぎ": "gi", "ぐ": "gu", "げ": "ge", "ご": "go",
    "ざ": "za", "じ": "ji", "ず": "zu", "ぜ": "ze", "ぞ": "zo",
    "だ": "da", "ぢ": "ji", "づ": "zu", "で": "de", "ど": "do",
    "ば": "ba", "び": "bi", "ぶ": "bu", "べ": "be", "ぼ": "bo",
    "ぱ": "pa", "ぴ": "pi", "ぷ": "pu", "ぺ": "pe", "ぽ": "po",
    "ぁ": "a", "ぃ": "i", "ぅ": "u", "ぇ": "e", "ぉ": "o",
    "ゔ": "vu",
}

DIGRAPH_ROMAJI = {
    "きゃ": "kya", "きゅ": "kyu", "きょ": "kyo",
    "しゃ": "sha", "しゅ": "shu", "しょ": "sho",
    "ちゃ": "cha", "ちゅ": "chu", "ちょ": "cho",
    "にゃ": "nya", "にゅ": "nyu", "にょ": "nyo",
    "ひゃ": "hya", "ひゅ": "hyu", "ひょ": "hyo",
    "みゃ": "mya", "みゅ": "myu", "みょ": "myo",
    "りゃ": "rya", "りゅ": "ryu", "りょ": "ryo",
    "ぎゃ": "gya", "ぎゅ": "gyu", "ぎょ": "gyo",
    "じゃ": "ja", "じゅ": "ju", "じょ": "jo",
    "びゃ": "bya", "びゅ": "byu", "びょ": "byo",
    "ぴゃ": "pya", "ぴゅ": "pyu", "ぴょ": "pyo",
    "ふぁ": "fa", "ふぃ": "fi", "ふぇ": "fe", "ふぉ": "fo",
    "てぃ": "ti", "でぃ": "di", "うぃ": "wi", "うぇ": "we", "うぉ": "wo",
    "しぇ": "she", "ちぇ": "che", "じぇ": "je",
}

SYMBOLS = {
    "　": " ", " ": " ", "。": ".", "、": ",", "！": "!", "？": "?",
    "：": ":", "；": ";", "（": "(", "）": ")", "「": '"', "」": '"',
    "『": '"', "』": '"', "・": " ", "〜": "-", "～": "-", "ー": "-",
    "①": "1", "②": "2", "③": "3", "④": "4", "⑤": "5",
}

RUBY_PATTERN = re.compile(r"\[\[[^|]+\|([^\]]+)\]\]")


def to_hiragana(char: str) -> str:
    if char == "ヴ":
        return "ゔ"
    code = ord(char)
    if 0x30A1 <= code <= 0x30F6:
        return chr(code - 0x60)
    return char


def romanize_at(text: str, index: int) -> tuple[str, int] | None:
    for length in (2, 1):
        segment = "".join(to_hiragana(char) for char in text[index:index + length])
        if segment in DIGRAPH_ROMAJI:
            return DIGRAPH_ROMAJI[segment], length
        if length == 1 and segment in BASE_ROMAJI:
            return BASE_ROMAJI[segment], 1
    return None


def is_space_or_punctuation(char: str) -> bool:
    return char == "" or char.isspace() or char in SYMBOLS


def is_particle(text: str, index: int, hira: str) -> bool:
    previous_char = to_hiragana(text[index - 1]) if index > 0 else ""
    next_char = to_hiragana(text[index + 1]) if index + 1 < len(text) else ""

    if hira == "は":
        return is_space_or_punctuation(next_char) or previous_char in {"で", "に", "と"}
    if hira == "を":
        return True
    if hira == "へ":
        return is_space_or_punctuation(next_char)
    return False


def append_particle(result: list[str], particle: str, next_char: str) -> None:
    if result and result[-1] != " ":
        result.append(" ")
    result.append(particle)
    if not is_space_or_punctuation(next_char):
        result.append(" ")


def to_romaji(text: str) -> str:
    text = RUBY_PATTERN.sub(r"\1", text)
    text = text.replace("[[", "").replace("]]", "")

    result: list[str] = []
    index = 0
    while index < len(text):
        char = text[index]
        hira = to_hiragana(char)

        if is_particle(text, index, hira):
            append_particle(result, {"は": "wa", "を": "wo", "へ": "e"}[hira], text[index + 1] if index + 1 < len(text) else "")
            index += 1
            continue

        if hira == "っ":
            next_value = romanize_at(text, index + 1)
            if next_value:
                result.append(next_value[0][0])
            index += 1
            continue

        if char in SYMBOLS:
            result.append(SYMBOLS[char])
            index += 1
            continue

        value = romanize_at(text, index)
        if value:
            romaji, consumed = value
            result.append(romaji)
            index += consumed
            continue

        result.append(char)
        index += 1

    output = "".join(result)
    output = re.sub(r"\s+", " ", output)
    output = re.sub(r"\s+([.,!?;:])", r"\1", output)
    output = output.replace("konnichi wa", "konnichiwa")
    return output.strip()


def process_file(path: Path) -> None:
    data = json.loads(path.read_text(encoding="utf-8"))
    for lesson in data.get("lessons", []):
        for sentence in lesson.get("sentences", []):
            sentence["romaji"] = to_romaji(sentence.get("jp", ""))
    path.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(f"Updated romaji in {path}")


ROOT = Path(os.environ.get("JPLEARN_ROOT", Path(__file__).resolve().parents[4]))
FILES = [
    ROOT / "server" / "JPLearn.Infrastructure" / "Data" / "Imports" / "speaking" / "jpd113.lessons.json",
    ROOT / "server" / "JPLearn.Infrastructure" / "Data" / "Imports" / "speaking" / "jpd123.lessons.json",
]

for file_path in FILES:
    if file_path.exists():
        process_file(file_path)
