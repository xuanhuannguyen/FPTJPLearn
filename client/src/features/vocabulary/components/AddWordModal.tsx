import React, { useState } from 'react';
import { X, Loader2 } from 'lucide-react';
import { vocabularyApi } from '../api/vocabularyApi';

interface AddWordModalProps {
  isOpen: boolean;
  listId: string;
  onClose: () => void;
  onSuccess: (newItem: any) => void;
}

export const AddWordModal: React.FC<AddWordModalProps> = ({ isOpen, listId, onClose, onSuccess }) => {
  const [formData, setFormData] = useState({
    word: '',
    reading: '',
    type: 'Danh từ',
    meaning: '',
    example: '',
    exampleMeaning: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState('');

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.word || !formData.reading || !formData.meaning) {
      setError('Please fill in Word, Reading, and Meaning fields.');
      return;
    }

    try {
      setIsSubmitting(true);
      setError('');
      // API call
      const res = await vocabularyApi.addItem(listId, formData);
      
      // Pass back a simulated item structure to update UI instantly without re-fetching
      onSuccess({
        id: res.itemId,
        word: formData.word,
        reading: formData.reading,
        wordType: formData.type,
        meaning: formData.meaning,
        exampleSentence: formData.example,
        exampleMeaning: formData.exampleMeaning,
        orderIndex: Number.MAX_SAFE_INTEGER,
        level: 0,
        status: 'new'
      });
      
      onClose();
      // Reset form
      setFormData({
        word: '', reading: '', type: 'Danh từ', meaning: '', example: '', exampleMeaning: ''
      });
    } catch (err: any) {
      setError(err.response?.data?.title || 'Failed to add word.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const wordTypes = ['Danh từ', 'Động từ nhóm 1', 'Động từ nhóm 2', 'Động từ nhóm 3', 'Tính từ đuôi i', 'Tính từ đuôi na', 'Trạng từ', 'Khác'];

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-text-primary/20 backdrop-blur-sm animate-fade-in">
      <div className="bg-bg-secondary w-full max-w-md rounded-2xl shadow-card flex flex-col border border-border">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-border bg-bg-primary/50">
          <h2 className="text-xl font-bold text-text-primary">Add New Word</h2>
          <button onClick={onClose} className="p-2 rounded-full text-text-secondary hover:bg-bg-tertiary">
            <X size={20} />
          </button>
        </div>

        {/* Body */}
        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          {error && <div className="text-sm text-accent-danger bg-accent-danger/10 p-3 rounded-xl">{error}</div>}
          
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-text-secondary mb-1">Word *</label>
              <input 
                type="text" 
                value={formData.word}
                onChange={e => setFormData({...formData, word: e.target.value})}
                placeholder="e.g. 食べる"
                className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-text-secondary mb-1">Reading *</label>
              <input 
                type="text" 
                value={formData.reading}
                onChange={e => setFormData({...formData, reading: e.target.value})}
                placeholder="e.g. たべる"
                className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-text-secondary mb-1">Meaning *</label>
              <input 
                type="text" 
                value={formData.meaning}
                onChange={e => setFormData({...formData, meaning: e.target.value})}
                placeholder="e.g. Ăn"
                className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-text-secondary mb-1">Type *</label>
              <select 
                value={formData.type}
                onChange={e => setFormData({...formData, type: e.target.value})}
                className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary"
              >
                {wordTypes.map(t => <option key={t} value={t}>{t}</option>)}
              </select>
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-text-secondary mb-1">Example Sentence</label>
            <input 
              type="text" 
              value={formData.example}
              onChange={e => setFormData({...formData, example: e.target.value})}
              placeholder="e.g. 私はりんごを食べる。"
              className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary mb-2"
            />
            <input 
              type="text" 
              value={formData.exampleMeaning}
              onChange={e => setFormData({...formData, exampleMeaning: e.target.value})}
              placeholder="e.g. Tôi ăn táo."
              className="w-full p-2.5 rounded-xl border border-border bg-bg-primary text-text-primary focus:outline-none focus:border-accent-primary"
            />
          </div>

          <div className="pt-4 flex justify-end gap-3">
            <button type="button" onClick={onClose} className="px-5 py-2.5 rounded-xl font-medium text-text-secondary hover:bg-bg-tertiary">
              Cancel
            </button>
            <button 
              type="submit" 
              disabled={isSubmitting}
              className="px-6 py-2.5 rounded-xl font-medium text-white bg-accent-primary hover:bg-accent-hover shadow-glow hover:-translate-y-0.5 disabled:opacity-50 transition-all flex items-center gap-2"
            >
              {isSubmitting && <Loader2 size={18} className="animate-spin" />}
              Save Word
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
