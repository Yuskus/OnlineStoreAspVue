<script>
  export default {
    name: "OnlineStore",
    data() {
      return {
        sitename: 'Online Shop',
        username: '',
        role: ''
      }
    },
    methods: {
      getUserInfo() {
        // написать получение юзера
        // как получить себя?
        // нужен будет GUID, name, role

        // завершить баскет, ордерс юзерс (запросы)
        // поправить апи (постранички)
        // доделать стили
        // сделать модалки для редактирования и показывать опции, которые доступны по роли (редактирование или просмотр)
      },
      logout() {
        localStorage.removeItem('jwt');
      }
    }
  }
</script>

<template>
  <header>
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
    <div class="panel">
      <div class="sitename">
        <h1>{{ sitename }}</h1>
      </div>
      <div class="userinfo">
        <h2>{{ username }}</h2>
        <h3>{{ role }}</h3>
        <a href="/auth" @click="logout()" class="button"><p>Выход</p></a>
      </div>
    </div>
    <div class="menu" v-if="role == 'Менеджер'">
      <div><router-link class="link" to="/">Главная</router-link></div>
      <div><router-link class="link" to="/catalog">Каталог</router-link></div>
      <div><router-link class="link" to="/orders">Заказы</router-link></div>
      <div><router-link class="link" to="/users">Пользователи</router-link></div>
      <div><router-link class="link" to="/about">О компании</router-link></div>
    </div>
    <div class="menu" v-else>
      <div><router-link class="link" to="/">Главная</router-link></div>
      <div><router-link class="link" to="/catalog">Каталог</router-link></div>
      <div><router-link class="link" to="/basket">Корзина</router-link></div>
      <div><router-link class="link" to="/orders">Заказы</router-link></div>
      <div><router-link class="link" to="/about">О компании</router-link></div>
      <div><router-link class="link" to="/users">Пользователи</router-link></div> <!--убрать-->
    </div>
  </header>

  <section class="content">
    <router-view />
  </section>

  <footer>
    <p>© Виноградова Юлия</p>
  </footer>
</template>

<style scoped>
  header {
    background-color: #85a4ca;
    color: #2d3e54;
    text-align: center;
    min-height: 10vh;
  }

  header .link {
    color: #cfdae5;
    text-decoration: none;
    font-size: 19px;
  }

  .content {
    min-height: 75vh;
  }

  header h1, h2, h3 {
    font-weight: 700;
    margin: 5px;
  }

  header h1 {
    font-size: 28px;
  }

  header h2 {
    font-size: 24px;
  }

  header h3 {
    font-size: 22px;
    font-weight: 500;
  }

  header .button {
    display: inline-block;
    background-color: #cbdbea;
    padding: 7px 15px;
    border-radius: 10px;
    margin: 5px;
    color: #2d3e54;
    text-decoration: none;
    transition: all 0.3s ease;
    cursor: default;
  }

  header .button:hover {
    background-color: #f0f5fa;
  }

  header .panel {
    display: flex;
    justify-content: space-between;
  }

  header .sitename {
    flex: 1;
    margin: 30px;
  }

  .userinfo {
    min-width: 350px;
    min-height: 10vh;
    font-family: Roboto, Arial, Helvetica, sans-serif, system-ui;
  }

  .menu {
    background-color: #a0bad4;
  }

  .menu div {
    display: inline-block;
    justify-content: space-around;
    padding: 20px;
    min-width: 120px;
    margin: 3px 7px;
    border-radius: 15px 0px;
    background-color: #3f5675;
    transition: transform .2s;
  }

  .menu div:hover {
    transform: scale(1.1);
  }

  footer {
    background-color: #3f5675;
    color: gray;
    text-align: center;
    min-height: 10vh;
    bottom: 0px;
  }

  footer p {
    padding: 20px 0px;
    font-size: 20px;
    font-weight: 400;
    color: #cfdae5;
  }

  header p {
    font-size: 18px;
  }

  h1, h2, h3, p, .link {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-weight: 500;
    font-style: normal;
  }
</style>
