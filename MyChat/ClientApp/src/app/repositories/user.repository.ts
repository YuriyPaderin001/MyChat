import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chat } from '../models/chat';
import { User } from '../models/user';

@Injectable()
export class UserRepository {
    private url = "/api/users";

    public constructor(private http: HttpClient) { }

    public getUsers() {
        return this.http.get(this.url);
    }

    public getUserById(id: number) {
        return this.http.get(this.url + '/' + id);
    }

    public addUser(user: User) {
        return this.http.post('/api/users', user);
    }
}