import { Injectable } from '@angular/core';
import { Note } from '../../Models/Note';
import { HttpClient } from '@angular/common/http';
import { AskResponse } from '../../Models/AskResponse';

@Injectable({
  providedIn: 'root',
})
export class NotesService {
  private url = 'https://localhost:7112/api/notes';
  constructor(private http: HttpClient) {}

  getNotes() {
    return this.http.get<Note[]>(this.url);
  }

  createNote(note: Note) {
    return this.http.post<Note>(this.url, note);
  }

  searchNotes(term: string) {
    return this.http.get<Note[]>(
      `${this.url}?search=${encodeURIComponent(term)}`,
    );
  }

  ask(question: string) {
    return this.http.post<AskResponse>(`${this.url}ai/brainstorm`, { question: question });
  }

  getHistory(){
    return this.http.get<AskResponse[]>(`${this.url}ai/history`);
  }

  deleteNote(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
