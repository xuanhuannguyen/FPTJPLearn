import json
import re

# Reverse Map for Hiragana -> Romaji
# Only for common Vietnamese words that might have been converted
REVERSE_MAP = {
    'むあ': 'mua',
    'な': 'na',
    'に': 'ni',
    'ね': 'ne',
    'の': 'no',
    'は': 'ha',
    'へ': 'he',
    'ほ': 'ho',
    'ま': 'ma',
    'み': 'mi',
    'め': 'me',
    'も': 'mo',
    'ら': 'ra',
    'れ': 're',
    'ろ': 'ro',
    'わ': 'wa',
    'た': 'ta',
    'て': 'te',
    'と': 'to',
    'さ': 'sa',
    'せ': 'se',
    'そ': 'so',
    'か': 'ka',
    'き': 'ki',
    'く': 'ku',
    'け': 'ke',
    'こ': 'ko',
    'が': 'ga',
    'げ': 'ge',
    'ご': 'go',
    'だ': 'da',
    'で': 'de',
    'ど': 'do'
}

# Standard Romaji to Hiragana Mapping
ROMAJI_MAP = {
    'a': 'あ', 'i': 'い', 'u': 'う', 'e': 'え', 'o': 'お',
    'ka': 'か', 'ki': 'き', 'ku': 'く', 'ke': 'け', 'ko': 'こ',
    'sa': 'さ', 'shi': 'し', 'su': 'す', 'se': 'せ', 'so': 'そ',
    'ta': 'た', 'chi': 'ち', 'tsu': 'つ', 'te': 'て', 'to': 'と',
    'na': 'な', 'ni': 'に', 'nu': 'ぬ', 'ne': 'ね', 'no': 'の',
    'ha': 'は', 'hi': 'ひ', 'fu': 'ふ', 'he': 'へ', 'ho': 'ほ',
    'ma': 'ま', 'mi': 'み', 'mu': 'む', 'me': 'め', 'mo': ' mo',
    'ya': 'や', 'yu': 'ゆ', 'yo': 'よ',
    'ra': 'ら', 'ri': 'り', 'ru': 'る', 're': 'れ', 'ro': 'ろ',
    'wa': 'わ', 'wo': 'を', 'n': 'ん',
    'ga': 'が', 'gi': 'ぎ', 'gu': 'ぐ', 'ge': 'げ', 'go': 'ご',
    'za': 'ざ', 'ji': 'じ', 'zu': 'ず', 'ze': 'ぜ', 'zo': 'ぞ',
    'da': 'だ', 'di': 'ぢ', 'du': 'づ', 'de': 'で', 'do': 'ど',
    'ba': 'ば', 'bi': 'び', 'bu': 'ぶ', 'be': 'べ', 'bo': 'ぼ',
    'pa': 'ぱ', 'pi': 'ぴ', 'pu': 'ぷ', 'pe': 'ぺ', 'po': 'ぽ',
    'kya': 'きゃ', 'kyu': 'きゅ', 'kyo': 'きょ',
    'sha': 'しゃ', 'shu': 'しゅ', 'sho': 'しょ',
    'cha': 'ちゃ', 'chu': 'ちゅ', 'cho': 'ちょ',
    'nya': 'にゃ', 'nyu': 'にゅ', 'nyo': 'にょ',
    'hya': 'ひゃ', 'hyu': 'ひゅ', 'hyo': 'ひょ',
    'mya': 'みゃ', 'myu': 'みゅ', 'myo': 'みょ',
    'rya': 'りゃ', 'ryu': 'りゃ', 'ryo': 'りょ',
    'gya': 'ぎゃ', 'gyu': 'ぎゅ', 'gyo': 'ぎょ',
    'ja': 'じゃ', 'ju': 'じゅ', 'jo': 'じょ',
    'bya': 'びゃ', 'byu': 'びゅ', 'byo': 'びょ',
    'pya': 'ぴゃ', 'pyu': 'ぴゅ', 'pyo': 'ぴょ'
}

def convert_romaji_word(word):
    if not word: return ""
    word = word.lower()
    res = ""
    i = 0
    while i < len(word):
        if i + 1 < len(word) and word[i] == word[i+1] and word[i] not in 'aeioun':
            res += 'っ'
            i += 1
            continue
        found = False
        for l in [3, 2, 1]:
            if i + l <= len(word):
                chunk = word[i:i+l]
                if chunk in ROMAJI_MAP:
                    res += ROMAJI_MAP[chunk]
                    i += l
                    found = True
                    break
        if not found:
            res += word[i]
            i += 1
    return res

def repair_and_convert(text):
    if not text: return text
    
    # 1. REPAIR: Look for Hiragana words that shouldn't be in Vietnamese text
    # This is a heuristic: if a Hiragana word is surrounded by Vietnamese text (spaces)
    # and it's in our REVERSE_MAP, change it back.
    def restore_match(match):
        word = match.group(0)
        if word in REVERSE_MAP:
            return REVERSE_MAP[word]
        return word

    # Pattern: Match 1-3 Hiragana chars surrounded by spaces or Vietnamese start/end
    text = re.sub(r'(?<=\s)[\u3041-\u309F]{1,3}(?=\s|\.|\,)', restore_match, text)

    # 2. CONVERT: Only convert if word is in quotes or parentheses
    def replace_match(match):
        prefix = match.group(1)
        word = match.group(2)
        suffix = match.group(3)
        return prefix + convert_romaji_word(word) + suffix

    # Target words in quotes or parentheses like (kaban), 'ni', "depato", “kyoukai”
    # Also handle dashes in parens like (デパート - depato)
    text = re.sub(r"([(\'\"“\s-])([a-zA-Z]{2,})([)\'\"”\s])", replace_match, text)
    
    return text

def convert_file(path):
    print(f"Repairing and converting {path}...")
    with open(path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    changed = 0
    for item in data:
        old_exp = item.get("explanation", "")
        if old_exp:
            new_exp = repair_and_convert(old_exp)
            if new_exp != old_exp:
                item["explanation"] = new_exp
                changed += 1
    
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Done. Processed {changed} explanations.")

# Paths
jpd113 = r'b:\FPT\Project\FPT_JPD\material\JPD113\JPD113_Full_Bank.json'
jpd123 = r'b:\FPT\Project\FPT_JPD\material\JPD123\jpd123_questions_final.json'

if __name__ == "__main__":
    convert_file(jpd113)
    convert_file(jpd123)
