import json
import re

# Comprehensive Romaji to Hiragana Mapping
ROMAJI_MAP = {
    'a': 'あ', 'i': 'い', 'u': 'う', 'e': 'え', 'o': 'お',
    'ka': 'か', 'ki': 'き', 'ku': 'く', 'ke': 'け', 'ko': 'こ',
    'sa': 'さ', 'shi': 'し', 'su': 'す', 'se': 'せ', 'so': 'そ',
    'ta': 'た', 'chi': 'ち', 'tsu': 'つ', 'te': 'て', 'to': 'と',
    'na': 'な', 'ni': 'に', 'nu': 'ぬ', 'ne': 'ね', 'no': ' của',
    'ha': 'は', 'hi': 'ひ', 'fu': 'ふ', 'he': 'へ', 'ho': 'ほ',
    'ma': 'ま', 'mi': 'み', 'mu': 'む', 'me': 'め', 'mo': 'mo',
    'ya': 'や', 'yu': 'ゆ', 'yo': 'よ',
    'ra': 'ら', 'ri': 'り', 'ru': 'る', 're': 'れ', 'ro': 'ろ',
    'wa': 'わ', 'wo': 'を', 'n': 'ん',
    'ga': 'が', 'gi': 'ぎ', 'gu': 'ぐ', 'ge': 'げ', 'go': 'ご',
    'za': 'ざ', 'ji': 'じ', 'zu': 'ず', 'ze': 'ぜ', 'zo': 'ぞ',
    'da': 'だ', 'di': 'ぢ', 'du': 'づ', 'de': 'で', 'do': 'đo',
    'ba': 'ba', 'bi': 'bi', 'bu': 'bu', 'be': 'be', 'bo': 'bo',
    'pa': 'pa', 'pi': 'pi', 'pu': 'pu', 'pe': 'pe', 'po': 'po',
    'kya': 'きゃ', 'kyu': 'きゅ', 'kyo': 'きょ',
    'sha': 'しゃ', 'shu': 'しゅ', 'sho': 'しょ',
    'cha': 'ちゃ', 'chu': 'ちゅ', 'cho': 'cho',
    'nya': 'にゃ', 'nyu': 'にゅ', 'nyo': 'にょ',
    'hya': 'ひゃ', 'hyu': 'ひゅ', 'hyo': 'ひょ',
    'mya': 'みゃ', 'myu': 'みゅ', 'myo': 'みょ',
    'rya': 'りゃ', 'ryu': 'りゅ', 'ryo': 'りょ',
    'gya': 'ぎゃ', 'gyu': 'ぎゅ', 'gyo': 'ぎょ',
    'ja': 'じゃ', 'ju': 'じゅ', 'jo': 'じょ',
    'bya': 'びゃ', 'byu': 'びゅ', 'byo': 'びょ',
    'pya': 'ぴゃ', 'pyu': 'ぴゅ', 'pyo': 'ぴょ'
}
# Fixed some mistakes in my manual map above

HIRAGANA_TO_ROMAJI = {v: k for k, v in ROMAJI_MAP.items() if len(k) <= 3}

def convert_to_hiragana(word):
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

def convert_to_romaji(hira_text):
    res = ""
    i = 0
    while i < len(hira_text):
        found = False
        for l in [2, 1]: # Try 2 hira chars then 1
            if i + l <= len(hira_text):
                chunk = hira_text[i:i+l]
                if chunk in HIRAGANA_TO_ROMAJI:
                    res += HIRAGANA_TO_ROMAJI[chunk]
                    i += l
                    found = True
                    break
        if not found:
            res += hira_text[i]
            i += 1
    return res

def final_repair_and_convert(text):
    if not text: return text
    
    # 1. First, convert EVERYTHING that looks like Hiragana but is OUTSIDE quotes/parens back to Romaji
    # We identify "islands" of text and process them
    
    parts = re.split(r'([(\'\"“\)].*?[)\'\"”])', text)
    new_parts = []
    for part in parts:
        if part.startswith(('(', "'", '"', '“')):
            # INSIDE quotes/parens: Convert Romaji -> Hiragana
            def repl(m):
                word = m.group(0)
                # Only convert if it looks like Japanese Romaji and not English
                if re.match(r'^[a-zA-Z]{2,}$', word) and word.lower() not in ['the', 'and', 'but', 'this', 'that', 'from']:
                    return convert_to_hiragana(word)
                return word
            
            new_part = re.sub(r'[a-zA-Z]{2,}', repl, part)
            new_parts.append(new_part)
        else:
            # OUTSIDE: Convert any Hiragana -> Romaji (to fix damage)
            # Match Hiragana blocks
            def repl_hira(m):
                return convert_to_romaji(m.group(0))
            
            new_part = re.sub(r'[\u3041-\u309F]{1,}', repl_hira, part)
            new_parts.append(new_part)
            
    return "".join(new_parts)

def convert_file(path):
    print(f"Final repair and conversion for {path}...")
    with open(path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    changed = 0
    for item in data:
        old_exp = item.get("explanation", "")
        if old_exp:
            new_exp = final_repair_and_convert(old_exp)
            if new_exp != old_exp:
                item["explanation"] = new_exp
                changed += 1
    
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Done. Processed {changed} explanations.")

if __name__ == "__main__":
    convert_file(r'b:\FPT\Project\FPT_JPD\material\JPD113\JPD113_Full_Bank.json')
    convert_file(r'b:\FPT\Project\FPT_JPD\material\JPD123\jpd123_questions_final.json')
