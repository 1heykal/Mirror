import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Note } from '../../Models/Note';

@Component({
  selector: 'app-note-card',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './note-card.component.html',
  styleUrl: './note-card.component.scss',
})
export class NoteCardComponent {
  @Input() note!: Note;
  @Input() isDeleting = false;
  @Output() deleteRequested = new EventEmitter<number>();

  showDeleteConfirm = false;

  onDeleteClick(): void {
    this.showDeleteConfirm = true;
  }

  onConfirmDelete(): void {
    if (this.note.id) {
      this.deleteRequested.emit(this.note.id);
      this.showDeleteConfirm = false;
    }
  }

  onCancelDelete(): void {
    this.showDeleteConfirm = false;
  }
}
