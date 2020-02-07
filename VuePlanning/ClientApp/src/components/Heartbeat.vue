<template>
  <div class="heart">
    <slot></slot>
  </div>
</template>
<script>
export default {
  name: "Heartbeat"
};
</script>
<style>
@keyframes heartbeat {
  0% {
    transform: scale(1);
  }

  20% {
    transform: scale(1.05) translateX(2%) translateY(2%);
  }

  40% {
    transform: scale(1.1) translateX(3%) translateY(3%);
  }
}
.heart {
  animation: heartbeat 1s infinite;
}
</style>
