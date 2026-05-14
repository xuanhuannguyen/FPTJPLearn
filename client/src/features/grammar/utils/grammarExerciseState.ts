import type { GrammarExerciseCheckResult } from '../types/grammar.types';

export type GrammarExerciseState = {
  answerText: string;
  selectedOptionIndexes: number[];
  result?: GrammarExerciseCheckResult;
  revealedAnswer?: string;
  error?: string;
};

export const emptyGrammarExerciseState: GrammarExerciseState = {
  answerText: '',
  selectedOptionIndexes: [],
};

export const getGrammarExerciseState = (
  state: Record<string, GrammarExerciseState>,
  exerciseId: string
) => ({
  ...emptyGrammarExerciseState,
  ...state[exerciseId],
});

