import { useCallback, useEffect, useState } from 'react';
import { Volume2, VolumeX } from 'lucide-react';
import type { ReviewCard } from '../types';

type VocabularyAudioControlProps = {
  card: ReviewCard;
  compact?: boolean;
  tone?: 'dark' | 'light';
};

const canUseSpeech = () => typeof window !== 'undefined' && 'speechSynthesis' in window;

export const VocabularyAudioControl = ({
  card,
  compact = false,
  tone = 'light',
}: VocabularyAudioControlProps) => {
  const [isSpeaking, setIsSpeaking] = useState(false);
  const supported = canUseSpeech();
  const speechText = card.reading || card.word;

  useEffect(() => {
    return () => {
      if (canUseSpeech()) {
        window.speechSynthesis.cancel();
      }
    };
  }, [card.itemId]);

  const speak = useCallback(() => {
    if (!supported) {
      return;
    }

    window.speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(speechText);
    utterance.lang = 'ja-JP';
    utterance.rate = 0.82;
    utterance.pitch = 1;
    utterance.onstart = () => setIsSpeaking(true);
    utterance.onend = () => setIsSpeaking(false);
    utterance.onerror = () => setIsSpeaking(false);

    window.speechSynthesis.speak(utterance);
  }, [speechText, supported]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.target instanceof HTMLInputElement || event.target instanceof HTMLTextAreaElement) {
        return;
      }

      if (event.key.toLowerCase() === 'v') {
        event.preventDefault();
        speak();
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [speak]);

  const stop = () => {
    if (!supported) {
      return;
    }

    window.speechSynthesis.cancel();
    setIsSpeaking(false);
  };

  const buttonTone = tone === 'dark'
    ? 'border-white/10 bg-white/10 text-white/75 hover:bg-white/15 hover:text-white'
    : 'border-border bg-white text-blue-700 shadow-pop hover:bg-blue-50 hover:text-blue-800';

  return (
    <button
      type="button"
      onClick={isSpeaking ? stop : speak}
      disabled={!supported}
      className={`inline-flex items-center justify-center gap-2 rounded-2xl border-2 font-extrabold transition-all disabled:cursor-not-allowed disabled:opacity-45 ${buttonTone} ${
        compact ? 'h-10 px-3 text-xs' : 'min-h-11 px-4 text-sm'
      }`}
      aria-label={isSpeaking ? 'Dừng đọc từ vựng' : `Đọc từ ${card.word}`}
      title={supported ? 'Đọc phát âm tiếng Nhật' : 'Trình duyệt không hỗ trợ đọc tự động'}
    >
      {isSpeaking ? <VolumeX size={compact ? 16 : 18} /> : <Volume2 size={compact ? 16 : 18} />}
      {!compact && <span>{isSpeaking ? 'Dừng đọc' : 'Đọc từ'}</span>}
    </button>
  );
};
