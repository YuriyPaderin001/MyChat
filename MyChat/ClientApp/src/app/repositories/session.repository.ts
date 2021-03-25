import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginData } from '../models/login.data';

@Injectable()
export class SessionRepository {
    private url = "/api/users";

    public constructor(private http: HttpClient) { }

    public login(loginData: LoginData) {
        return this.http.post('/api/sessions', loginData);
    }

    public logout() {
        this.http.delete('/api/sessions');
    }
}