import React, { useState } from 'react';
import { isAxiosError } from 'axios';
import { X, CheckCircle2, AlertCircle, Loader2 } from 'lucide-react';
import { vocabularyApi } from '../api/vocabularyApi';
import type { ImportVocabularyDto } from '../api/vocabularyApi';

interface ImportModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

type ApiValidationErrorResponse = {
  title?: string;
  errors?: Record<string, string[]>;
};

export const ImportModal: React.FC<ImportModalProps> = ({ isOpen, onClose, onSuccess }) => {
  const [jsonInput, setJsonInput] = useState('');
  const [status, setStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');
  const [errorMessage, setErrorMessage] = useState('');

  if (!isOpen) return null;

  const handleImport = async () => {
    try {
      setStatus('loading');
      setErrorMessage('');
      
      const parsedData = JSON.parse(jsonInput) as Partial<ImportVocabularyDto>;
      
      // Basic validation
      if (!parsedData.name || !parsedData.words || !Array.isArray(parsedData.words)) {
        throw new Error('Invalid format. JSON must include "name" and a "words" array.');
      }

      await vocabularyApi.importJSON(parsedData as ImportVocabularyDto);
      
      setStatus('success');
      setTimeout(() => {
        onSuccess();
        onClose();
        setJsonInput('');
        setStatus('idle');
      }, 1500);
      
    } catch (err: unknown) {
      setStatus('error');
      // Parse ASP.NET Core validation errors
      let errorMsg = err instanceof Error ? err.message : 'Failed to parse JSON or import to server.';
      if (isAxiosError<ApiValidationErrorResponse>(err) && err.response?.data?.errors) {
        const validationErrors = Object.values(err.response.data.errors).flat().join(', ');
        errorMsg = `Validation failed: ${validationErrors}`;
      } else if (isAxiosError<ApiValidationErrorResponse>(err) && err.response?.data?.title) {
        errorMsg = err.response.data.title;
      }
      setErrorMessage(errorMsg);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-text-primary/20 backdrop-blur-sm animate-fade-in">
      <div className="clay-modal max-h-[90vh] max-w-2xl">
        {/* Header */}
        <div className="clay-modal-header">
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
          <label htmlFor="import-json-input" className="mb-2 block text-sm font-extrabold text-text-secondary">
            Vocabulary JSON
          </label>
          <textarea
            id="import-json-input"
            value={jsonInput}
            onChange={(e) => setJsonInput(e.target.value)}
            placeholder={`{\n  "name": "N4_Bài 1",\n  "description": "Từ vựng Minna no Nihongo",\n  "words": [\n    {\n      "word": "行きます",\n      "reading": "いきます",\n      "type": "Động từ nhóm 1",\n      "meaning": "Đi"\n    }\n  ]\n}`}
            className="form-control h-64 resize-none font-mono text-sm"
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
        <div className="clay-modal-footer p-6">
          <button 
            onClick={onClose}
            className="btn-secondary"
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
