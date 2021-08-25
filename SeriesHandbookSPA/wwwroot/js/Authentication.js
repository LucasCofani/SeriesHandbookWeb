class FirebaseAuthHelper {
    constructor() {
        this.finished = false;
        this.token = "";
        this.created = false;
    }
    CreateUser(email, pass) {
        let parent = this;
        firebase.auth()
            .createUserWithEmailAndPassword(email, pass)
            .then((userCredential) => {
                parent.finished = true;
                parent.created = true;
            })
            .catch((error) => {
                console.log(error);
                parent.finished = true;
            });
    }
    LoginCredentials(email, pass) {
        let parent = this;
        firebase.auth().signInWithEmailAndPassword(email, pass)
            .then(() => {
                firebase
                    .auth()
                    .currentUser
                    .getIdToken(true)
                    .then((jwtToken) => {
                        parent.finished = true;
                        parent.token = jwtToken;
                    }).catch((error) => {
                        parent.finished = true;
                    });
                firebase.auth().signOut();
            })
            .catch((error) => {
                console.log(error);
                parent.finished = true;
            });
    }
    Login(provider) {
        let parent = this;
        firebase
            .auth()
            .signInWithPopup(provider)
            .then(() => {
                firebase
                    .auth()
                    .currentUser
                    .getIdToken(true)
                    .then((jwtToken) => {
                        parent.finished = true;
                        parent.token = jwtToken;
                    }).catch((error) => {
                        parent.finished = true;
                    });
                firebase.auth().signOut();
            })
            .catch((error) => {
                parent.finished = true;
            });
    }
}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
var firebaseConfig = {
    apiKey: "AIzaSyANF8GvxOEVuBhfAJiQDm8edrXcVnQCDyw",
    authDomain: "serieshandbook.firebaseapp.com",
    projectId: "serieshandbook",
    storageBucket: "serieshandbook.appspot.com",
    messagingSenderId: "141271687780",
    appId: "1:141271687780:web:17cfa2dade59bb3a607571",
    measurementId: "G-FKMPEJW819"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
firebase.auth().setPersistence(firebase.auth.Auth.Persistence.NONE);


export async function LoginGoogle() {

    let loginModel = new FirebaseAuthHelper();

    let providerGoogle = new firebase.auth.GoogleAuthProvider();

    // Explicit ask for account
    providerGoogle.setCustomParameters({
        prompt: 'select_account'
    });

    // What access we are asking
    providerGoogle.addScope("profile");

    loginModel.Login(providerGoogle);
    while (!loginModel.finished) {
        await sleep(1000);
    }
    return loginModel.token;
}
export async function LoginCredentials(user, pass) {
    let loginModel = new FirebaseAuthHelper();

    loginModel.LoginCredentials(user, pass);
    while (!loginModel.finished) {
        await sleep(1000);
    }
    return loginModel.token;
}
export async function CreateUser(user, pass) {
    let loginModel = new FirebaseAuthHelper();
    loginModel.CreateUser(user, pass);
    while (!loginModel.finished) {
        await sleep(1000);
    }
    return loginModel.created;
}
