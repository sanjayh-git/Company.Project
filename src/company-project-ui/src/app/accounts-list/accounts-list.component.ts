import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Account } from '../models/account';

@Component({
  selector: 'app-accounts-list',
  templateUrl: './accounts-list.component.html',
  styleUrls: ['./accounts-list.component.scss']
})
export class AccountsListComponent implements OnInit {
  accounts: Account[] = [];
  loading = false;
  error: string | null = null;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.loading = true;
    this.error = null;

    this.api.getAccounts().subscribe({
      next: accounts => {
        this.accounts = accounts;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load accounts';
        this.loading = false;
      }
    });
  }
}
