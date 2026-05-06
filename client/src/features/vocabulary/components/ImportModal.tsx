import React, { useState } from 'react';
import { X, CheckCircle2, AlertCircle, Loader2 } from 'lucide-react';
import { vocabularyApi } from '../api/vocabularyApi';

interface ImportModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const ImportModal: React.FC<ImportModalProps> = ({ isOpen, onClose, onSuccess }) => {
  const [jsonInput, setJsonInput] = useState('');
  const [status, setStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');
  const [errorMessage, setErrorMessage] = useState('');

  if (!isOpen) return null;

  const handleImport = async () => {
    try {
      setStatus('loading');
      setErrorMessage('');
      
      const parsedData = JSON.parse(jsonInput);
      
      // Basic validation
      if (!parsedData.name || !parsedData.words || !Array.isArray(parsedData.words)) {
        throw new Error('Invalid format. JSON must include "name" and a "words" array.');
      }

      await vocabularyApi.importJSON(parsedData);
      
      setStatus('success');
      setTimeout(() => {
        onSuccess();
        onClose();
        setJsonInput('');
        setStatus('idle');
      }, 1500);
      
    } catch (err: any) {
      setStatus('error');
      // Parse ASP.NET Core validation errors
      let errorMsg = err.message || 'Failed to parse JSON or import to server.';
      if (err.response?.data?.errors) {
        const validationErrors = Object.values(err.response.data.errors).flat().join(', ');
        errorMsg = `Validation failed: ${validationErrors}`;
      } else if (err.response?.data?.title) {
        errorMsg = err.response.data.title;
      }
      setErrorMessage(errorMsg);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-text-primary/20 backdrop-blur-sm animate-fade-in">
      <div className="bg-bg-secondary w-full max-w-2xl rounded-2xl shadow-card flex flex-col max-h-[90vh] overflow-hidden border border-border">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-border bg-bg-primary/50">
          <div>
            <h2 className="text-xl font-bold text-text-primary">Import JSON</h2>
            <p className="text-sm text-text-secondary mt-1">Paste your vocabulary JSON array below.</p>
          </div>
          <button 
            onClick={onClose}
            className="p-2 rounded-full text-text-secondary hover:bg-bg-tertiary hover:text-text-primary transition-colors"
          >
            <X size={20} />
          </button>
        </div>

        {/* Body */}
        <div className="p-6 flex-1 overflow-y-auto">
          <textarea
            value={jsonInput}
            onChange={(e) => setJsonInput(e.target.value)}
            placeholder={`{\n  "name": "N4_Bài 1",\n  "description": "Từ vựng Minna no Nihongo",\n  "words": [\n    {\n      "word": "行きます",\n      "reading": "いきます",\n      "type": "Động từ nhóm 1",\n      "meaning": "Đi"\n    }\n  ]\n}`}
            className="w-full h-64 p-4 rounded-xl border border-border bg-bg-primary text-text-primary font-mono text-sm focus:outline-none focus:border-accent-primary focus:ring-1 focus:ring-accent-primary transition-all resize-none placeholder:text-text-muted"
          />

          {status === 'error' && (
            <div className="mt-4 p-3 bg-accent-danger/10 border border-accent-danger/20 rounded-xl flex items-start gap-3 text-accent-danger">
              <AlertCircle size={20} className="shrink-0 mt-0.5" />
              <span className="text-sm">{errorMessage}</span>
            </div>
          )}

          {status === 'success' && (
            <div className="mt-4 p-3 bg-accent-success/10 border border-accent-success/20 rounded-xl flex items-start gap-3 text-accent-success">
              <CheckCircle2 size={20} className="shrink-0 mt-0.5" />
              <span className="text-sm">Imported successfully!</span>
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="p-6 border-t border-border flex justify-end gap-3 bg-bg-primary/50">
          <button 
            onClick={onClose}
            className="px-5 py-2.5 rounded-xl font-medium text-text-secondary hover:bg-bg-tertiary transition-colors"
          >
            Cancel
          </button>
          <button 
            onClick={handleImport}
            disabled={!jsonInput.trim() || status === 'loading'}
            className="px-6 py-2.5 rounded-xl font-medium text-white bg-accent-primary hover:bg-accent-hover transition-all shadow-glow hover:-translate-y-0.5 disabled:opacity-50 disabled:hover:translate-y-0 disabled:shadow-none flex items-center gap-2"
          >
            {status === 'loading' && <Loader2 size={18} className="animate-spin" />}
            Import
          </button>
        </div>
      </div>
    </div>
  );
};
