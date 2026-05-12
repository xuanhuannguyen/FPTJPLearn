import json
import re

def clean_text(text):
    if not text:
        return text
    
    # Specific common corruptions found in the logs
    replacements = {
        'ん': 'n',
        'い': 'i',
        'う': 'u',
        'ほ': 'ho',
        'ぎ': 'gi',
        'け': 'ke',
        'せ': 'se',
        'た': 'ta',
        'ち': 'chi',
        'つ': 'tsu',
        'て': 'te',
        'と': 'to',
        'な': 'na',
        'に': 'ni',
        'ぬ': 'nu',
        'ね': 'ne',
        'の': 'no',
        'は': 'ha',
        'ひ': 'hi',
        'ふ': 'fu',
        'へ': 'he',
        'ま': 'ma',
        'み': 'mi',
        'む': 'mu',
        'め': 'me',
        'も': 'mo',
        'ら': 'ra',
        'り': 'ri',
        'る': 'ru',
        'れ': 're',
        'ろ': 'ro',
        'わ': 'wa',
        'を': 'wo'
    }

    # We only want to replace these when they are part of a Vietnamese word.
    # A Vietnamese word usually has Latin characters or tone marks.
    # Pattern: a Japanese character surrounded by Latin characters or tone marks.
    
    # Tone marks and Vietnamese specific chars
    vn_chars = "àáảãạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđÀÁẢÃẠĂẰẮẲẴẶÂẦẤẨẪẬÈÉẺẼẸÊỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỊĐ"
    latin_chars = "a-zA-Z"
    combined = latin_chars + vn_chars
    
    # First, handle the most common single character insertions
    # e.g. tháんg -> tháng, sいんh -> sinh
    for jp, lat in replacements.items():
        # Match jp character when surrounded by Vietnamese/Latin characters
        # Or at the end of a Vietnamese word
        pattern = f'([{combined}])({jp})|({jp})([{combined}])'
        
        def replace_match(m):
            if m.group(1): # Left side match
                return m.group(1) + lat
            else: # Right side match
                return lat + m.group(4)
        
        text = re.sub(pattern, replace_match, text)

    # Some specific multi-char ones
    text = text.replace('qうốc', 'quốc')
    text = text.replace('tいếんg', 'tiếng')
    text = text.replace('んhật', 'nhật')
    text = text.replace('sいんh', 'sinh')
    text = text.replace('tháんg', 'tháng')
    text = text.replace('mấy ぎờ', 'mấy giờ')
    text = text.replace('điện tほại', 'điện thoại')
    text = text.replace('tầんg', 'tầng')
    text = text.replace('んgười', 'người')
    text = text.replace('qうầy', 'quầy')
    text = text.replace('tいếp', 'tiếp')
    text = text.replace('tương ứんg', 'tương ứng')
    text = text.replace('nghĩa là', 'nghĩa là') # check
    
    # Generic fix for 'ん' in Vietnamese words (very common)
    text = re.sub(f'([{combined}])ん', r'\1n', text)
    text = re.sub(f'ん([{combined}])', r'n\1', text)
    
    return text

def process_file(filepath):
    print(f"Processing {filepath}...")
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    if 'questions' in data:
        for q in data['questions']:
            q['questionText'] = clean_text(q['questionText'])
            q['explanation'] = clean_text(q['explanation'])
            if 'options' in q:
                for opt in q['options']:
                    opt['text'] = clean_text(opt['text'])
    
    if 'passages' in data:
        for p in data['passages']:
            p['content'] = clean_text(p['content'])
            p['title'] = clean_text(p['title'])

    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Done {filepath}")

if __name__ == "__main__":
    process_file(r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd113.questions.json')
    process_file(r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd123.questions.json')
