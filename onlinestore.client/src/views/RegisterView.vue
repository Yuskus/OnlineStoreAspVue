<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="auth">
    <div class="input-fields">
      <label class="to-center">Registration Form</label>

      <label>Enter Username:</label>
      <input type="text" v-model="customer.username" placeholder="Username" required />

      <label>Enter Password:</label>
      <input type="password" v-model="customer.password" placeholder="Password" required />

      <label>Enter Customer Name:</label>
      <input type="text" v-model="customer.name" placeholder="Customer Name" required />

      <label>Enter Customer Code:</label>
      <input type="text" v-model="customer.code" placeholder="Customer Code" required />
      <label>Enter Customer Address:</label>
      <input type="text" v-model="customer.address" placeholder="Customer Address" />
    </div>
    <div class="btns">
      <button @click="register()" class="accent">Зарегистрироваться</button>
      <a href="/auth" >Войти</a>
    </div>
  </div>
</template>

<script>
  import usersApi from '../api/usersApi';
  
  export default {
    name: "Register",
    data() {
      return {
        customer: {
          name: "",
          code: "",
          address: null,
          username: "",
          password: ""
        }
      }
    },
    methods: {
      makeRegisterCustomerRequest() {
        return {
          customerRequest: {
            name: this.customer.name,
            code: this.customer.code,
            address: this.customer.address
          },
          username: this.customer.username,
          password: this.customer.password
        };
      },
      async register() {
        try {
          let newCustomer = this.makeRegisterCustomerRequest();
          const response = await usersApi.registerCustomer(newCustomer);
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
        alert('Непредвиденная ошибка!');
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
    padding: 30px;
  }

  .input-fields {
    align-content: center;
    justify-self: center;
    padding-bottom: 50px;
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

  .to-center {
    justify-self: center;
    font-size: 20px;
    font-weight: 500;
  }

  label {
    display: block;
    line-height: 40px;
    margin-top: 15px;
    color: #1c2633;
    font-weight: 500;
    text-shadow: 1px 1px rgba(0,0,0,0.1);
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
    padding-bottom: 20px;
  }

  .accent {
    background-color: rgba(0, 0, 50, 0.05);
  }

  a, button {
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

  .accent:hover {
    background-color: rgba(0, 0, 50, 0.2);
  }
</style>
