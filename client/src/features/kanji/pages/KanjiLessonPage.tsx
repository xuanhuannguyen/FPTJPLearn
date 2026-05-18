import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { ArrowLeft, Play, LayoutGrid, Keyboard } from 'lucide-react';
import { kanjiApi } from '../api/kanjiApi';
import type { KanjiLesson, KanjiItem, KanjiVocabulary } from '../types/kanji.types';
import { PremiumLock } from '../../../shared/components/PremiumLock';
import { useUserAccess } from '../../../shared/hooks/useUserAccess';


export const KanjiLessonPage = () => {
  const { level, lessonId } = useParams<{ level: string; lessonId: string }>();
  const [lesson, setLesson] = useState<KanjiLesson | null>(null);
  const [kanjis, setKanjis] = useState<KanjiItem[]>([]);
  const [vocabularies, setVocabularies] = useState<KanjiVocabulary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const { isContentLocked } = useUserAccess();

  useEffect(() => {
    const fetchData = async () => {
      if (!lessonId) return;
      try {
        const [lessonData, kanjiData, vocabData] = await Promise.all([
          kanjiApi.getLessonById(lessonId),
          kanjiApi.getKanjiItemsByLesson(lessonId),
          kanjiApi.getVocabularyByLesson(lessonId),
        ]);
        setLesson(lessonData);
        setKanjis(kanjiData);
        setVocabularies(vocabData);
      } catch (error) {
        console.error('Failed to fetch lesson details:', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [lessonId]);

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-10 w-10 animate-spin border-4 border-black border-t-transparent"></div>
      </div>
    );
  }

  if (!lesson) {
    return <div className="p-8 text-center font-mono">Lesson not found</div>;
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-4 animate-fade-in space-y-6">
      <PremiumLock isLocked={isContentLocked(lesson)} packageCode={lesson.packageCode}>
        {/* Header Panel */}
        <div className="flex flex-col md:flex-row md:items-end justify-between border-b-2 border-black pb-4 gap-4">
          <div>
            <div className="mb-2 flex items-center gap-2">
              <Link 
                to={`/kanji/${level}`}
                className="flex h-7 w-7 items-center justify-center border border-black hover:bg-black hover:text-white transition-colors"
              >
                <ArrowLeft size={14} />
              </Link>
              <span className="bg-black px-2 py-0.5 text-[10px] font-black uppercase text-white font-mono">
                {level} • L{lesson.lessonNumber}
              </span>
            </div>
            <h1 className="text-3xl font-black uppercase tracking-tight text-text-primary">
              {lesson.title}
            </h1>
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6">
          {/* Left Column: Kanji Items */}
          <div className="lg:col-span-2 space-y-4">
            <div className="flex items-center justify-between border border-black bg-slate-50 px-3 py-2">
              <h2 className="text-sm font-black uppercase tracking-wider">Kanji Core ({kanjis.length})</h2>
              <div className="flex gap-2">
                <Link 
                  to={`/kanji/${level}/lessons/${lessonId}/study`}
                  className="flex items-center gap-1.5 border border-black bg-accent-primary px-3 py-1.5 text-[11px] font-black uppercase text-white hover:bg-black hover:shadow-[2px_2px_0px_#F97316] transition-all shadow-[1px_1px_0px_#0F172A]"
                >
                  <Play size={12} fill="currentColor" />
                  Study Mode
                </Link>
                <Link 
                  to={`/kanji/${level}/lessons/${lessonId}/flashcards`}
                  className="flex items-center gap-1.5 border border-black bg-white px-3 py-1.5 text-[11px] font-black uppercase hover:bg-slate-100 transition-all shadow-[1px_1px_0px_#0F172A]"
                >
                  <LayoutGrid size={12} />
                  Flashcard
                </Link>
              </div>
            </div>
            
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              {kanjis.map((kanji, index) => (
                <Link
                  key={kanji.id}
                  to={`/kanji/${level}/lessons/${lessonId}/study?index=${index}`}
                  className="group flex border border-black bg-white p-2 hover:border-accent-primary hover:shadow-[2px_2px_0px_#F97316] transition-all cursor-pointer"
                >
                  {/* Large Kanji Display */}
                  <div className="flex h-20 w-20 shrink-0 items-center justify-center border-r border-black bg-slate-50">
                    <span className="text-5xl font-serif text-text-primary" style={{ fontFamily: '"Noto Serif JP", serif' }}>
                      {kanji.character}
                    </span>
                  </div>
                  
                  {/* Details */}
                  <div className="flex flex-col justify-center px-3 flex-1 min-w-0">
                    <div className="flex items-baseline gap-2 mb-1">
                      <span className="text-xs font-black text-text-primary uppercase">{kanji.hanViet}</span>
                      <span className="text-[10px] text-text-secondary truncate">{kanji.meaning}</span>
                    </div>
                    
                    <div className="flex flex-wrap gap-1 mt-1">
                      {kanji.kunReading && (
                        <span className="bg-slate-100 border border-border/50 px-1.5 py-0.5 text-[10px] font-mono text-text-secondary">
                          {kanji.kunReading}
                        </span>
                      )}
                      {kanji.onReading && (
                        <span className="bg-blue-50 border border-blue-200 px-1.5 py-0.5 text-[10px] font-mono text-accent-primary">
                          {kanji.onReading}
                        </span>
                      )}
                    </div>
                  </div>
                </Link>
              ))}
            </div>
          </div>

          {/* Right Column: Vocabulary Items */}
          <div className="space-y-4">
            <div className="flex items-center justify-between border border-black bg-slate-50 px-3 py-2">
              <h2 className="text-sm font-black uppercase tracking-wider">Vocabulary ({vocabularies.length})</h2>
              <Link 
                to={`/kanji/${level}/lessons/${lessonId}/vocabulary-flashcards`}
                className="flex items-center gap-1.5 border border-black bg-white px-3 py-1.5 text-[11px] font-black uppercase hover:bg-slate-100 transition-all shadow-[1px_1px_0px_#0F172A]"
              >
                <LayoutGrid size={12} />
                Flashcard
              </Link>
              <Link
                to={`/kanji/${level}/lessons/${lessonId}/vocabulary-flashcards?mode=typing`}
                className="flex items-center gap-1.5 border border-black bg-white px-3 py-1.5 text-[11px] font-black uppercase hover:bg-slate-100 transition-all shadow-[1px_1px_0px_#0F172A]"
              >
                <Keyboard size={12} />
                Gõ
              </Link>
            </div>
            
            <div className="flex flex-col gap-2">
              {vocabularies.map((vocab) => (
                <div 
                  key={vocab.id} 
                  className="flex items-center justify-between border border-black bg-white p-2"
                >
                  <div className="flex flex-col">
                    <span className="text-lg font-bold text-text-primary">
                      {vocab.word}
                    </span>
                    <span className="text-xs text-text-secondary font-mono">
                      {vocab.reading}
                    </span>
                  </div>
                  <div className="text-right">
                    <span className="text-xs font-medium text-text-primary block max-w-[120px] truncate">
                      {vocab.meaning}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </PremiumLock>
    </div>

  );
};
