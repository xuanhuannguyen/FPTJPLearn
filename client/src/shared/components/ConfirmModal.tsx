import { useState, type FC } from 'react';
import { createPortal } from 'react-dom';
import { AlertTriangle } from 'lucide-react';

interface ConfirmModalProps {
  isOpen: boolean;
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  onConfirm: () => void | Promise<void>;
  onCancel: () => void;
  isDestructive?: boolean;
  errorMessage?: string;
}

export const ConfirmModal: FC<ConfirmModalProps> = ({
  isOpen,
  title,
  message,
  confirmText = 'Confirm',
  cancelText = 'Cancel',
  onConfirm,
  onCancel,
  isDestructive = true,
  errorMessage
}) => {
  const [isProcessing, setIsProcessing] = useState(false);

  if (!isOpen) return null;

  const handleConfirm = async () => {
    try {
      setIsProcessing(true);
      await onConfirm();
      onCancel();
    } catch (error) {
      console.error(error);
    } finally {
      setIsProcessing(false);
    }
  };

  return createPortal(
    <div
      className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-text-primary/20 backdrop-blur-sm animate-fade-only"
      role="dialog"
      aria-modal="true"
      aria-labelledby="confirm-modal-title"
    >
      <div className="clay-modal max-w-sm animate-modal-in">
        <div className="p-6">
          <div className="flex items-center gap-3 mb-4">
            <div className={`icon-badge h-12 w-12 ${isDestructive ? 'bg-accent-danger/10 text-accent-danger' : 'bg-accent-primary/10 text-accent-primary'}`}>
              <AlertTriangle size={24} />
            </div>
            <h2 id="confirm-modal-title" className="text-xl font-bold text-text-primary">{title}</h2>
          </div>
          <p className="text-text-secondary">{message}</p>
          {errorMessage && (
            <p className="mt-3 rounded-lg bg-accent-danger/10 px-3 py-2 text-sm text-accent-danger">
              {errorMessage}
            </p>
          )}
        </div>
        
        <div className="clay-modal-footer">
          <button 
            onClick={onCancel}
            disabled={isProcessing}
            className="btn-secondary min-h-10 px-4 py-2 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {cancelText}
          </button>
          <button 
            onClick={handleConfirm}
            disabled={isProcessing}
            className={`rounded-xl border-2 border-border px-4 py-2 font-extrabold text-white transition-all shadow-pop hover:-translate-y-0.5 disabled:cursor-not-allowed disabled:opacity-60 disabled:hover:translate-y-0 ${
              isDestructive ? 'bg-accent-danger hover:bg-red-600' : 'bg-accent-primary hover:bg-accent-hover'
            }`}
          >
            {isProcessing ? 'Deleting...' : confirmText}
          </button>
        </div>
      </div>
    </div>,
    document.body
  );
};
