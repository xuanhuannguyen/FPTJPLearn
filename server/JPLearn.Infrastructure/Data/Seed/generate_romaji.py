import json
import re
import os

# Simplified Kana to Romaji mapping
KANA_ROMAN = {
    'あ': 'a', 'い': 'i', 'う': 'u', 'え': 'e', 'お': 'o',
    'か': 'ka', 'き': 'ki', 'く': 'ku', 'け': 'ke', 'こ': 'ko',
    'さ': 'sa', 'し': 'shi', 'す': 'su', 'せ': 'se', 'そ': 'so',
    'た': 'ta', 'ち': 'chi', 'つ': 'tsu', 'て': 'te', 'と': 'to',
    'な': 'na', 'に': 'ni', 'ぬ': 'nu', 'ね': 'ne', 'の': 'no',
    'は': 'ha', 'ひ': 'hi', 'ふ': 'fu', 'へ': 'he', 'ほ': 'ho',
    'ま': 'ma', 'み': 'mi', 'む': 'mu', 'め': 'me', 'も': 'mo',
    'や': 'ya', 'ゆ': 'yu', 'よ': 'yo',
    'ら': 'ra', 'り': 'ri', 'る': 'ru', 'れ': 're', 'ろ': 'ro',
    'わ': 'wa', 'を': 'wo', 'ん': 'n',
    'が': 'ga', 'ぎ': 'gi', 'ぐ': 'gu', 'げ': 'ge', 'ご': 'go',
    'ざ': 'za', 'じ': 'ji', 'ず': 'zu', 'ぜ': 'ze', 'ぞ': 'zo',
    'だ': 'da', 'ぢ': 'ji', 'づ': 'zu', 'で': 'de', 'ど': 'do',
    'ば': 'ba', 'び': 'bi', 'ぶ': 'bu', 'べ': 'be', 'ぼ': 'bo',
    'ぱ': 'pa', 'ぴ': 'pi', 'ぷ': 'pu', 'ぺ': 'pe', 'ぽ': 'po',
    'きゃ': 'kya', 'きゅ': 'kyu', 'きょ': 'kyo',
    'しゃ': 'sha', 'しゅ': 'shu', 'しょ': 'sho',
    'ちゃ': 'cha', 'ちゅ': 'chu', 'ちょ': 'cho',
    'にゃ': 'nya', 'にゅ': 'nyu', 'にょ': 'nyo',
    'ひゃ': 'hya', 'ひゅ': 'hyu', 'ひょ': 'hyo',
    'みゃ': 'mya', 'みゅ': 'myu', 'みょ': 'myo',
    'りゃ': 'rya', 'りゅ': 'ryu', 'りょ': 'ryo',
    'ぎゃ': 'gya', 'ぎゅ': 'gyu', 'ぎょ': 'gyo',
    'じゃ': 'ja', 'じゅ': 'ju', 'じょ': 'jo',
    'びゃ': 'bya', 'びゅ': 'byu', 'びょ': 'byo',
    'ぴゃ': 'pya', 'ぴゅ': 'pyu', 'ぴょ': 'pyo',
    'っ': 'SMALL_TSU',
    'ー': '-', ' ': ' ', '！': '!', '？': '?', '。': '.', '、': ',',
    'ア': 'a', 'イ': 'i', 'ウ': 'u', 'エ': 'e', 'オ': 'o',
    'カ': 'ka', 'キ': 'ki', 'ク': 'ku', 'ケ': 'ke', 'コ': 'ko',
    'サ': 'sa', 'シ': 'shi', 'ス': 'su', 'セ': 'se', 'ソ': 'so',
    'タ': 'ta', 'チ': 'chi', 'ツ': 'tsu', 'テ': 'te', 'ト': 'to',
    'ナ': 'na', 'ニ': 'ni', 'ヌ': 'nu', 'ネ': 'ne', 'ノ': 'no',
    'ハ': 'ha', 'ヒ': 'hi', 'フ': 'fu', 'ヘ': 'he', 'ホ': 'ho',
    'マ': 'ma', 'ミ': 'mi', 'ム': 'mu', 'メ': 'me', 'モ': 'mo',
    'ヤ': 'ya', 'ユ': 'yu', 'ヨ': 'yo',
    'ラ': 'ra', 'リ': 'ri', 'ル': 'ru', 'レ': 're', 'ロ': 'ro',
    'ワ': 'wa', 'ヲ': 'wo', 'ン': 'n',
    'ガ': 'ga', 'ギ': 'gi', 'グ': 'gu', 'ゲ': 'ge', 'ご': 'go',
    'ザ': 'za', 'ジ': 'ji', 'ズ': 'zu', 'ゼ': 'ze', 'ゾ': 'zo',
    'ダ': 'da', 'ヂ': 'ji', 'ヅ': 'zu', 'デ': 'de', 'ド': 'do',
    'バ': 'ba', 'ビ': 'bi', 'ブ': 'bu', 'ベ': 'be', 'ボ': 'bo',
    'パ': 'pa', 'ピ': 'pi', 'プ': 'pu', 'ペ': 'pe', 'ポ': 'po',
}

def to_romaji(text):
    # Extract Furigana if present: [[Kanji|Furigana]] -> Furigana
    text = re.sub(r'\[\[[^|]+\|([^\]]+)\]\]', r'\1', text)
    # Remove remaining brackets
    text = text.replace('[[', '').replace(']]', '')
    
    res = ""
    i = 0
    while i < len(text):
        char = text[i]
        next_char = text[i+1] if i + 1 < len(text) else ""
        
        # Check for 2-char combinations (kya, sha, etc)
        combined = char + next_char
        if combined in KANA_ROMAN:
            val = KANA_ROMAN[combined]
            res += val
            i += 2
        elif char in KANA_ROMAN:
            val = KANA_ROMAN[char]
            if val == 'SMALL_TSU':
                # Handle double consonants
                i += 1
                if i < len(text):
                    next_val = to_romaji(text[i])
                    if next_val:
                        res += next_val[0]
                continue
            
            # Special case for particles
            if char == 'は' and (i == 0 or text[i-1] in ' 　'): # Very naive check
                 pass # usually handled manually or by context
            
            res += val
            i += 1
        else:
            res += char
            i += 1
    
    # Post-process particles and common cases
    # (In a real app, use a proper tokenizer like Mecab/Sudachi)
    # We will just do basic replacements for this speaking task
    res = res.replace('ha', ' wa ') # Naive particle fix
    res = res.replace('  ', ' ').strip()
    return res

def process_file(path):
    with open(path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    for lesson in data.get('lessons', []):
        for sentence in lesson.get('sentences', []):
            jp = sentence.get('jp', '')
            sentence['romaji'] = to_romaji(jp)
            
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Added romaji to {path}")

files = [
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\speaking\jpd113.lessons.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\speaking\jpd123.lessons.json"
]

for f in files:
    if os.path.exists(f):
        process_file(f)
