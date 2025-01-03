import { Expense } from './expense.model';

export class Category {
  categoryId: number =0;
  name: string = '';
  description: string = '';
  expenses: Expense[] =[];

  
}