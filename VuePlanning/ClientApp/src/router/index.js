import Vue from "vue";
import VueRouter from "vue-router";
import Login from "../views/Login.vue";

Vue.use(VueRouter);
function load(name) {
  return () => import(/* webpackChunkName: "v-[request]" */`../views/${name}.vue`);
}
const routes = [
  {
    path: "/login",
    name: "login",
    component: Login
  },
  {
    path: "/room",
    name: "room",
    component: load("Room")
  },
  { path: "*", redirect: "login" }
  //{
  //path: "/about",
  //name: "about",
  // route level code-splitting
  // this generates a separate chunk (about.[hash].js) for this route
  // which is lazy-loaded when the route is visited.
  //component: () => import(/* webpackChunkName: "about" */ "../views/About.vue")
  //}
];

const router = new VueRouter({
  mode: "history",
  routes
});

export default router;
