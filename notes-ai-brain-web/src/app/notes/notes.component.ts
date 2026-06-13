import { Component, inject } from '@angular/core';
import { NotesService } from '../../services/notes/notes.service';
import { Note } from '../../Models/Note';
import { CommonModule, DatePipe, AsyncPipe } from '@angular/common';
import { Output } from '@angular/core';
import { CreateNoteComponent } from '../create-note/create-note.component';
import { NoteCardComponent } from '../note-card/note-card.component';
import { FormsModule } from '@angular/forms';
import { AskResponse } from '../../Models/AskResponse';
import { finalize } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-notes',
  standalone: true,
  imports: [CommonModule, CreateNoteComponent, NoteCardComponent, FormsModule],
  templateUrl: './notes.component.html',
  styleUrl: './notes.component.scss',
})
export class NotesComponent {
  private notesService = inject(NotesService);
  notes: Note[] = [];
  searchTerm: string = '';
  question: string = '';
  answer: string = '';

  isAsking = false;

  history: AskResponse[] = [];

  ngOnInit(): void {
    this.getNotes();
    this.getHistory();
  }

  getNotes() {
    this.notesService.getNotes().subscribe((notes: Note[]) => {
      this.notes = notes;
    });
  }

  getHistory() {
    this.notesService.getHistory().subscribe((history: AskResponse[]) => {
      this.history = history;
    });
  }

  onNoteCreated(note: Note) {
    this.notes.push(note);
  }

  deletingNoteId: number | null = null;
  deleteMessage: { type: 'success' | 'error'; text: string } | null = null;

  onDeleteNote(noteId: number): void {
    this.deletingNoteId = noteId;
    this.notesService.deleteNote(noteId).subscribe({
      next: () => {
        this.notes = this.notes.filter((n) => n.id !== noteId);
        this.deletingNoteId = null;
        this.deleteMessage = {
          type: 'success',
          text: 'Note deleted successfully.',
        };
        setTimeout(() => {
          this.deleteMessage = null;
        }, 3000);
      },
      error: (error: HttpErrorResponse) => {
        this.deletingNoteId = null;
        this.deleteMessage = {
          type: 'error',
          text: 'Failed to delete note. Please try again.',
        };
        console.error('Delete error:', error);
      },
    });
  }

  searchNotes() {
    this.notesService
      .searchNotes(this.searchTerm.trim())
      .subscribe((notes: Note[]) => {
        if (notes) this.notes = notes;
        else this.getNotes();
      });
  }

  ask() {
    if (this.isAsking) return;

    this.isAsking = true;

    this.notesService
      .ask(this.question.trim())
      .pipe(
        finalize(() => {
          this.isAsking = false;
        }),
      )
      .subscribe({
        next: (response: AskResponse) => {
          this.answer = response.answer;
          response.question = this.question;
          response.askedAt = new Date(Date.now());
          this.history.push(response);
          this.question = '';
        },

        error: (error: HttpErrorResponse) => {
          this.answer = 'Something went wrong. Please try again.';
        },
      });
  }

  expandedRows: boolean[] = [];

  toggleExpand(index: number): void {
    this.expandedRows[index] = !this.expandedRows[index];
  }
}
