import { initializeApp } from 'firebase/app';
import { getAuth, GoogleAuthProvider } from 'firebase/auth';

const firebaseConfig = {
  apiKey: "AIzaSyC007dF0jBXXYiKs3WwUtDjEZRvCFGxA04",
  authDomain: "jpd-eacda.firebaseapp.com",
  projectId: "jpd-eacda",
  storageBucket: "jpd-eacda.firebasestorage.app",
  messagingSenderId: "375985707097",
  appId: "1:375985707097:web:b9289903eb5f179d5b7948",
  measurementId: "G-V7Y5VTPK55"
};

const app = initializeApp(firebaseConfig);
export const auth = getAuth(app);
export const googleProvider = new GoogleAuthProvider();
