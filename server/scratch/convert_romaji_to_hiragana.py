import json
import re

ROMAJI_MAP = {
    'a': 'あ', 'i': 'い', 'u': 'う', 'e': 'え', 'o': 'お',
    'ka': 'か', 'ki': 'き', 'ku': 'く', 'ke': 'け', 'ko': 'こ',
    'sa': 'さ', 'shi': 'し', 'su': 'す', 'se': 'せ', 'so': 'そ',
    'ta': 'た', 'chi': 'ち', 'tsu': 'つ', 'te': 'て', 'to': 'と',
    'na': 'な', 'ni': 'に', 'nu': 'ぬ', 'ne': 'ね', 'no': 'の',
    'ha': 'は', 'hi': 'ひ', 'fu': 'ふ', 'he': 'へ', 'ho': 'ほ',
    'ma': 'ま', 'mi': 'み', 'mu': 'む', 'me': 'め', 'mo': 'も',
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
    'rya': 'りゃ', 'ryu': 'りゅ', 'ryo': 'りょ',
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

def process_explanation(text):
    if not text: return text
    
    # 1. Handle words in quotes or parentheses (even with spaces or symbols before)
    # This pattern matches words made of letters that are clearly Japanese-like
    def replace_romaji(match):
        word = match.group(0)
        # Check if it looks like Romaji (vowels mixed with consonants)
        # Avoid English words
        vowels = set('aeiou')
        has_vowel = any(c in vowels for c in word.lower())
        if has_vowel and len(word) >= 2:
            # Don't convert if it's likely English
            if word.lower() not in ['masu', 'the', 'and', 'but', 'this', 'that', 'from', 'with', 'to', 'for', 'was', 'were']:
                return convert_romaji_word(word)
        return word

    # Pattern: Match English-looking words inside quotes or parentheses
    # or after a dash inside parentheses
    text = re.sub(r'(?<=[(\'\"“\s-])[a-zA-Z]{2,}(?=[)\'\"”\s])', replace_romaji, text)
    
    return text

def convert_file(path):
    print(f"Processing {path}...")
    with open(path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    changed = 0
    for item in data:
        old_exp = item.get("explanation", "")
        if old_exp:
            new_exp = process_explanation(old_exp)
            if new_exp != old_exp:
                item["explanation"] = new_exp
                changed += 1
    
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Done. Changed {changed} explanations.")

# Paths
jpd113 = r'b:\FPT\Project\FPT_JPD\material\JPD113\JPD113_Full_Bank.json'
jpd123 = r'b:\FPT\Project\FPT_JPD\material\JPD123\jpd123_questions_final.json'

if __name__ == "__main__":
    convert_file(jpd113)
    convert_file(jpd123)
