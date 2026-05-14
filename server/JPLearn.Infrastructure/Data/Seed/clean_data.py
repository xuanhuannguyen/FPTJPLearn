import json
import re

def fix_speaking_113():
    path = r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\speaking\jpd113.lessons.json"
    with open(path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Typos
    content = content.replace("じゅうごにch", "じゅうごにち")
    content = content.replace("ビジネンこうえん", "ビジネスこうえん")
    content = content.replace("アンア", "アンナ")
    content = content.replace("ごぜん六時に ばんごはん", "ごご六時に ばんごはん")
    
    with open(path, 'w', encoding='utf-8') as f:
        f.write(content)
    print("Fixed speaking jpd113 typos.")

def normalize(t):
    if not t: return ""
    return re.sub(r'[\s\.。、,！？!?]', '', t)

def clean_exam_113():
    path = r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd113.questions.json"
    with open(path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    questions = data.get('questions', [])
    unique_questions = []
    seen = set()
    
    for q in questions:
        text = q.get('questionText', '')
        # Normalize for comparison
        norm_text = normalize(text)
        opts = tuple(sorted([normalize(opt['text']) for opt in q.get('options', [])]))
        sig = (norm_text, opts)
        
        # Global fix for Japanese punctuation
        if q.get('questionText'):
            q['questionText'] = q['questionText'].replace('. (', '。 (')
        
        # Specific fixes for known broken items
        if q['id'] == 9:
            q['options'] = [
                {"label": "A", "text": "はちがつ", "isCorrect": True},
                {"label": "B", "text": "しちがつ", "isCorrect": False},
                {"label": "C", "text": "くがつ", "isCorrect": False},
                {"label": "D", "text": "ろくがつ", "isCorrect": False}
            ]
        
        if q['id'] == 23:
            q['options'] = [
                {"label": "A", "text": "ゆうびんきょく", "isCorrect": True},
                {"label": "B", "text": "ぎんこう", "isCorrect": False},
                {"label": "C", "text": "としょかん", "isCorrect": False},
                {"label": "D", "text": "びょういん", "isCorrect": False}
            ]
            
        if sig not in seen:
            unique_questions.append(q)
            seen.add(sig)
    
    data['questions'] = unique_questions
    # Re-index orderIndex
    for i, q in enumerate(data['questions']):
        q['orderIndex'] = i + 1
        
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Cleaned exam jpd113. Removed {len(questions) - len(unique_questions)} duplicates.")

def fix_exam_123():
    path = r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd123.questions.json"
    with open(path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Encoding / Mixed mode fixes
    content = content.replace("JRno", "JRの")
    content = content.replace("DVDwo", "DVDを")
    content = content.replace("DVDha", "DVDは")
    content = content.replace(" को ", " を ")
    content = content.replace("ばo lâu", "bao lâu")
    content = content.replace("tiên ちゅ", "tiên chu")
    content = content.replace(". (", "。 (")
    
    data = json.loads(content)
    
    for q in data.get('questions', []):
        if q['id'] == 340:
            if len(q['options']) > 3 and q['options'][3]['text'] == "問きます":
                q['options'][3]['text'] = "歩きます"
    
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print("Fixed exam jpd123 encoding and logic errors.")

fix_speaking_113()
clean_exam_113()
fix_exam_123()
