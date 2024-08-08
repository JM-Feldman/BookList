import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  searchForm: FormGroup;

  constructor(
    private bookService: BookService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.searchForm = this.fb.group({
      title: [''],
      author: ['']
    });
  }

  ngOnInit(): void {
    this.bookService.getBooks().subscribe((data: Book[]) => {
      this.books = data;
    });
  }

  deleteBook(id: number): void {
    this.bookService.deleteBook(id).subscribe(() => {
      this.books = this.books.filter(book => book.id !== id);
    });
  }

  navigateToAddBook(): void {
    this.router.navigate(['/add-book']);
  }

  navigateToEditBook(id: number): void {
    this.router.navigate([`/edit-book/${id}`]);
  }

  navigateToDetailBook(id: number): void {
    this.router.navigate([`/book/${id}`]);
  }

  searchBooks(): void {
    const { title, author } = this.searchForm.value;
    this.bookService.searchBooks(title, author).subscribe((data: Book[]) => {
      this.books = data;
    });
  }
}

