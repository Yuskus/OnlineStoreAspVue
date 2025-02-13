<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="auth">
    <div class="input-fields">
      <h1>Login Form</h1>

      <AuthLineForm v-model="username" labelName="Enter Username:" :placeholderText="'Username'" :isRequired="true" />
      <AuthLineForm v-model="password" labelName="Enter Password:" :inputType="'password'" :placeholderText="'Password'" :isRequired="true" />

    </div>
    <div class="btns">
      <button @click.prevent="login()" class="accent">Войти</button>
      <a href="/auth/new">Зарегистрироваться</a>
    </div>
  </div>
</template>

<script>
  import { logIn } from '../api/usersApi';

  import AuthLineForm from '../components/forms/AuthLineForm.vue';

  export default {
    name: "Login",
    components: { AuthLineForm },
    data() {
      return {
        username: null,
        password: null
      }
    },
    methods: {
      makeLoginRequest() {
        return {
          username: this.username,
          password: this.password
        };
      },
      async login() {
        try {
          let loginForm = this.makeLoginRequest();
          const response = await logIn(loginForm);
          if (response) {
            localStorage.setItem('jwt', response.token);
            localStorage.setItem('username', response.username);
            localStorage.setItem('guid', response.customerId);
            localStorage.setItem('role', response.role);
            this.toMainPage();
          } else {
            localStorage.clear();
            alert('Неверный логин или пароль!');
          }
        } catch (error) {
          localStorage.clear();
          this.warnInfo('Ошибка при аутентификации (Login): ', error);
        }
      },
      toMainPage() {
        this.$router.push('/');
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert(error.message);
      }
    }
  }
</script>

<style scoped>
  .auth {
    background-color: white;
    min-width: 20vw;
    max-width: 350px;
    min-height: 400px;
    align-content: center;
    border-radius: 20px;
    justify-self: center;
    margin-top: 20vh;
    filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
  }

  .input-fields {
    padding: 30px 40px;
  }

  h1 {
    text-align: center;
    font-size: 20px;
    font-weight: 500;
    color: #1c2633;
    text-shadow: 1px 1px rgba(0,0,0,0.1);
    margin-top: 15px;
  }

  .btns {
    justify-self: center;
    padding: 20px;
  }

  a, button {
    display: inline-block;
    font-weight: 400;
    padding: 15px 25px;
    border-radius: 8px;
    margin: 5px;
    text-decoration: none;
    font-size: 16px;
  }

  button {
    border: none;
  }

  a:visited {
    color: #1c2633;
  }

  a:hover {
    background-color: rgba(0, 0, 50, 0.1);
  }

  .accent {
    background-color: rgba(0, 0, 50, 0.05);
  }

  .accent:hover {
    background-color: rgba(0, 0, 50, 0.2);
  }
</style>
