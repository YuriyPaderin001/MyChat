import { Component, EventEmitter, Output } from '@angular/core';
import { DataService } from './services/data.service';
import { LoginData } from './models/login.data';
import { User } from './models/user';

export enum AccountComponentState {
    AuthorizationPageShowing,
    RegistrationPageShowing
}

@Component({
    selector: 'account-component',
    templateUrl: './account.component.html',
    providers: [DataService]
})
export class AccountComponent {
    @Output() public onUserAuthorizated = new EventEmitter<User>();

    public State = AccountComponentState;
    public currentState = AccountComponentState.AuthorizationPageShowing;

    public loginData: LoginData = new LoginData();
    public confirmPassword: string;
    public user: User = new User();
    public errorMessage: string;

    constructor(private dataService: DataService) { }

    private authorizate() {
        this.errorMessage = null;
        this.dataService.session.login(this.loginData)
            .subscribe((data: User) => {
                if (data == null) {
                    this.errorMessage = "Неверное имя пользователя или пароль!";
                } else {
                    this.onUserAuthorizated.emit(data);
                }
            });
    }

    private registrate() {
        this.errorMessage = null;
        this.dataService.users.addUser(this.user)
            .subscribe((data: User) => {
                if (data == null) {
                    this.errorMessage = "Что-то пошло не так!";
                } else {
                    this.showAuthorizationPage();
                }
            });
    }

    public showAuthorizationPage() {
        this.errorMessage = null;
        this.loginData = new LoginData();
        this.setState(AccountComponentState.AuthorizationPageShowing);
    }

    public showRegistrationPage() {
        this.errorMessage = null;
        this.user = new User();
        this.setState(AccountComponentState.RegistrationPageShowing);
    }

    private setState(state: AccountComponentState) {
        this.currentState = state;
    }
}