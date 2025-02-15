<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="auth">
    <div class="input-fields">
      <h1>Registration Form</h1>

      <AuthLineForm v-model="username" labelName="Enter Username:" placeholderText="Username" :isRequired="true" />
      <AuthLineForm v-model="password" labelName="Enter Password:" inputType="password" placeholderText="Password" :isRequired="true" />
      <AuthLineForm v-model="name" labelName="Enter Customer Name:" placeholderText="Customer Name" :isRequired="true" />
      <AuthLineForm v-model="code" labelName="Enter Customer Code:" placeholderText="Customer Code" :isRequired="true" />
      <AuthLineForm v-model="address" labelName="Enter Customer Address:" placeholderText="Customer Address" :isRequired="false" />

    </div>
    <div class="btns">
      <button @click="register()" class="accent">Зарегистрироваться</button>
      <a href="/auth" >Войти</a>
    </div>
  </div>
</template>

<script>
  import { registerCustomer } from '../api/usersApi';
  
  import AuthLineForm from '../components/forms/AuthLineForm.vue';

  export default {
    name: "Register",
    components: { AuthLineForm },
    data() {
      return {
        name: "",
        code: "",
        address: null,
        username: "",
        password: ""
      }
    },
    methods: {
      makeRegisterCustomerRequest() {
        return {
          customerInfo: {
            name: this.name,
            code: this.code,
            address: this.address
          },
          username: this.username,
          password: this.password
        };
      },
      async register() {
        try {
          let newCustomer = this.makeRegisterCustomerRequest();
          console.log(newCustomer);
          const response = await registerCustomer(newCustomer);
          if (response) {
            this.toLoginPage();
          } else {
            alert('Ошибка регистрации. Попробуйте снова.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при регистрации (Register): ', error);
        }
      },
      toLoginPage() {
        this.$router.push('/auth');
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
    min-width: 25vw;
    max-width: 450px;
    min-height: 400px;
    align-content: center;
    border-radius: 20px;
    justify-self: center;
    margin-top: 12vh;
    filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
    padding: 20px;
  }

  .input-fields {
    padding: 30px 40px;
  }

  input {
    display: block;
    min-width: 15vw;
    min-height: 40px;
    justify-self: center;
    border: none;
    border-radius: 8px;
    outline: 1px solid rgba(28,38,51,0.3);
    padding: 4px;
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
    align-content: center;
    justify-self: center;
    padding-bottom: 20px;
  }
  
  a, button {
    font-size: 16px;
    font-weight: 400;
    padding: 15px 25px;
    border-radius: 8px;
    margin: 0px 15px;
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

  .accent {
    background-color: rgba(0, 0, 50, 0.05);
  }

  .accent:hover {
    background-color: rgba(0, 0, 50, 0.2);
  }
</style>
