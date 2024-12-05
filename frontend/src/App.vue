<template>
  <!-- <nav>
    <router-link to="/">Home</router-link> |
    <router-link to="/recommend">Recommend</router-link> |
    <router-link to="/market">Market</router-link> |
    <router-link to="/release">Release</router-link> |
    <router-link to="/my">My</router-link>
  </nav> -->
  <router-view/>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
