<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <UserEditWindow v-if="isOpenDialog" @close-dialog="clickWindowRedactor" :user="selectedUser" />
  <div class="container">
    <h1 class="line">Пользователи</h1>
    
    <div class="catalog">
      <div class="user" v-for="(user, index) in users" :key="index">
        <div class="user-desc">
          <label>ID Заказчика:</label>
          <div>{{ user.customer?.id ?? "Нет" }}</div>
        </div>
        <div class="user-desc">
          <label>Никнейм:</label>
          <div>{{ user.username }}</div>
        </div>
        <div class="user-desc">
          <label>Роль:</label>
          <div>{{ user.role == 1 ? "Менеджер" : "Заказчик" }}</div>
        </div>
        <div class="user-desc">
          <label>Полное имя:</label>
          <div>{{ user.customer?.name ?? "Нет" }}</div>
        </div>
        <div class="user-desc">
          <label>Код заказчика:</label>
          <div>{{ user.customer?.code ?? "Нет" }}</div>
        </div>
        <div class="user-desc">
          <label>Адрес:</label>
          <div>{{ user.customer?.address ?? "Нет" }}</div>
        </div>
        <div class="user-desc">
          <label>Персональная скидка:</label>
          <div>{{ user.customer?.discount ?? "Нет" }}</div>
        </div>
        <div class="user-desc">
          <label>Опции:</label>
          <div><a class="accent" @click="clickOnItem(index)">Редактирование</a></div>
        </div>
        </div>
      </div>

    <Pagination @page-changed="getUsers" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<script>
  import { getPageOfUsers } from '../api/usersApi';

  import Pagination from '../components/pagination/PaginationComponent.vue'
  import UserEditWindow from '../components/dialogs/UserEditWindow.vue';

  export default {
    name: 'Users',
    components: { Pagination, UserEditWindow },
    data() {
      return {
        currentPage: 1,
        totalPages: 1,
        pageSize: 24,
        totalCount: 0,
        users: [],
        selectedUser: null,
        isOpenDialog: false
      }
    },
    methods: {
      async getUsers(number = 1) {
        this.currentPage = number;
        try {
          const response = await getPageOfUsers(this.currentPage, this.pageSize);
          if (response) {
            this.users = response.userResponses;
            this.totalCount = response.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Ошибка во время получения списка пользователей.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при получении данных (Users): ', error);
        }
      },
      async clickOnItem(index) {
        this.selectedUser = this.users[index];
        await this.clickWindowRedactor(true);
      },
      async clickWindowRedactor(state) {
        this.isOpenDialog = state;
        if (!state) {
          await this.getUsers();
        }
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert('Непредвиденная ошибка!');
      }
    },
    mounted() {
      this.getUsers();
    }
  }
</script>

<style scoped>
  .container {
    max-width: 65vw;
    margin: 0px auto;
    padding-top: 20px;
    background-color: #e0ebf7;
    filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
  }

  .catalog {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-evenly;
  }

  div, label, h1, h3, p, a {
    font-weight: 500;
    color: #1c2633;
  }

  .accent {
    text-decoration: underline;
  }

  .accent:hover {
    color: #34547a;
  }

  label {
    color: #9199a2;
  }

  .user {
    align-items: center;
    box-shadow: 1px 2px 4px rgba(0, 50, 100, 0.2);
    align-content: space-around;
    margin: 10px;
    border-radius: 20px 20px 20px 20px;
    background-color: #f0f5fa;
    min-width: 300px;
    max-width: 400px;
  }

  .user div {
    align-self: center;
  }

  .user-desc {
    padding: 10px;
  }

  .user:hover {
    box-shadow: 4px 8px 16px rgba(0, 50, 100, 0.4);
  }

  h1 {
    font-size: 26px;
    line-height: 36px;
    padding: 20px 10px;
  }

  h3 {
    font-size: 22px;
    line-height: 30px;
    padding: 15px 10px;
  }

  p {
    font-size: 18px;
    line-height: 28px;
    padding: 0px 10px;
  }

  .line {
    background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
  }

  hr {
    border: none;
    border-top: 1px solid rgba(0,0,0,0.08);
    margin-top: 20px;
  }
</style>
