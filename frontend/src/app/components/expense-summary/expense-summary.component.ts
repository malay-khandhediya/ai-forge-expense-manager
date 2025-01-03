import { Component, OnInit } from '@angular/core';
import { ExpenseService } from '../../services/expense.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChartOptions, ChartType, ChartDataset } from 'chart.js';
// import { Label } from 'ng2-charts';

@Component({
  selector: 'app-expense-summary',
  templateUrl: './expense-summary.component.html',
  styleUrls: ['./expense-summary.component.css'],
  imports: [CommonModule, FormsModule],
})
export class ExpenseSummaryComponent implements OnInit {
  expenses: any[] = [];
  summary: { [key: string]: number } = {};
  expenseCategories: string[] = [];

  // Chart data
  public barChartOptions: ChartOptions = {
    responsive: true,
  };
  public barChartLabels: string[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [];
  public barChartData: ChartDataset[] = [
    { data: [], label: 'Expenses' }
  ];

  constructor(private expenseService: ExpenseService) {}

  ngOnInit() {
    this.loadExpenses();
  }

  loadExpenses() {
    this.expenseService.getExpenses().subscribe((data: any[]) => {
      this.expenses = data;
      this.calculateSummary();
      this.extractCategories();
      this.updateChartData();
    });
  }

  calculateSummary() {
    this.summary = {};
    this.expenses.forEach(expense => {
      const category = expense.category;
      if (!this.summary[category]) {
        this.summary[category] = 0;
      }
      this.summary[category] += expense.amount;
    });
  }

  extractCategories() {
    this.expenseCategories = [...new Set(this.expenses.map(expense => expense.category))];
  }

  updateChartData() {
    this.barChartLabels = Object.keys(this.summary);
    this.barChartData[0].data = Object.values(this.summary);
  }
}