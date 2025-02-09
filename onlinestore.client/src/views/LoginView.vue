<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="auth">
    <div class="input-fields">
      <label class="to-center">Login Form</label>

      <label>Enter Username:</label>
      <input type="text" v-model="username" placeholder="Username" required />

      <label>Enter Password:</label>
      <input type="password" v-model="password" placeholder="Password" required />
    </div>
    <div class="btns">
      <button @click.prevent="login()" class="accent">Войти</button>
      <a href="/auth/new">Зарегистрироваться</a>
    </div>
  </div>
</template>

<script>
  import usersApi from '../api/usersApi';

  export default {
    name: "Login",
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
          const response = await usersApi.logIn(loginForm);
          if (response) {
            localStorage.setItem('jwt', response.token);
            localStorage.setItem('username', response.username);
            localStorage.setItem('guid', response.customerId);
            localStorage.setItem('role', response.role);
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
        alert('Непредвиденная ошибка!');
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
    align-content: center;
    justify-self: center;
    padding-bottom: 30px;
  }

  input {
    display: block;
    min-width: 250px;
    min-height: 40px;
    justify-self: center;
    border: none;
    border-radius: 8px;
    outline: 1px solid rgba(28,38,51,0.3);
    padding: 4px;
  }

  .to-center {
    justify-self: center;
    font-size: 20px;
    font-weight: 500;
  }

  label {
    display: block;
    line-height: 40px;
    margin-top: 20px;
    color: #1c2633;
    text-shadow: 1px 1px rgba(0,0,0,0.1);
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-weight: 500;
    font-style: normal;
  }

  a, label, button {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-style: normal;
    font-size: 16px;
  }

  .btns {
    align-content: center;
    justify-self: center;
    padding: 20px;
  }

  .accent {
    background-color: rgba(0, 0, 50, 0.05);
  }

  a, button {
    display: inline-block;
    font-weight: 400;
    padding: 15px 25px;
    border-radius: 8px;
    margin: 5px;
    text-decoration: none;
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

  .accent:hover {
    background-color: rgba(0, 0, 50, 0.2);
  }
</style>
